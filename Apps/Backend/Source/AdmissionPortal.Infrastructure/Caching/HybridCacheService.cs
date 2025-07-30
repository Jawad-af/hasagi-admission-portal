using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AdmissionPortal.Infrastructure.Caching
{
    public class HybridCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<HybridCacheService> _logger;

        public HybridCacheService(IMemoryCache memoryCache, IDistributedCache distributedCache, ILogger<HybridCacheService> logger)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T? value))
            {
                _logger.LogInformation("MemoryCache hit for key '{CacheKey}'", key);
                return value;
            }

            _logger.LogInformation("MemoryCache miss for key '{CacheKey}'", key);
            var cachedString = await _distributedCache.GetStringAsync(key);

            if (cachedString == null)
            {
                _logger.LogInformation("RedisCache miss for key '{CacheKey}'", key);
                return default;
            }

            try
            {
                var deserialized = JsonSerializer.Deserialize<T>(cachedString);
                _memoryCache.Set(key, deserialized, TimeSpan.FromMinutes(5));
                _logger.LogInformation("RedisCache hit for key '{CacheKey}', backfilled MemoryCache", key);
                return deserialized;
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Failed to deserialize RedisCache value for key '{CacheKey}'", key);
                await RemoveAsync(key);
                return default;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            _memoryCache.Set(key, value, expiration ?? TimeSpan.FromMinutes(5));

            var serialized = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(10)
            };

            await _distributedCache.SetStringAsync(key, serialized, options);
            _logger.LogInformation("Set value in both MemoryCache and RedisCache for key '{CacheKey}'", key);
        }

        public async Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            await _distributedCache.RemoveAsync(key);
            _logger.LogInformation("Removed cache entry from both MemoryCache and RedisCache for key '{CacheKey}'", key);
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getData, TimeSpan? expiration = null)
        {
            if (_memoryCache.TryGetValue(key, out T? cachedValue) && cachedValue is not null)
            {
                _logger.LogInformation("MemoryCache hit in GetOrSet for key '{CacheKey}'", key);
                return cachedValue;
            }

            var redisValue = await _distributedCache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(redisValue))
            {
                try
                {
                    var deserialized = JsonSerializer.Deserialize<T>(redisValue);

                    if (deserialized is not null)
                    {
                        _memoryCache.Set(key, deserialized, expiration ?? TimeSpan.FromMinutes(5));
                        _logger.LogInformation("RedisCache hit in GetOrSet for key '{CacheKey}', backfilled MemoryCache", key);
                        return deserialized;
                    }
                    else
                    {
                        _logger.LogWarning("Deserialized Redis value is null for key '{CacheKey}', removing cache", key);
                        await RemoveAsync(key);
                    }
                }
                catch (JsonException ex)
                {
                    _logger.LogWarning(ex, "Failed to deserialize Redis value in GetOrSet for key '{CacheKey}'", key);
                    await RemoveAsync(key);
                }
            }

            _logger.LogInformation("Fetching data from source for key '{CacheKey}'", key);
            var freshData = await getData();
            await SetAsync(key, freshData, expiration);

            return freshData;
        }
    }
}
