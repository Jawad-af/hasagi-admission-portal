using AdmissionPortal.Infrastructure.Caching;

namespace AdmissionPortal.WebApi.Injectors
{
    public static class HybridCachingInjector
    {
        public static void InjectHybridCaching(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine("Started InjectHybridCaching");

            services.AddMemoryCache();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("Redis");
            });

            services.AddScoped<ICacheService, HybridCacheService>();

            Console.WriteLine("Finished InjectHybridCaching");
        }
    }
}
