using AdmissionPortal.Domain.Entities.Identity;
using AdmissionPortal.Domain.Entities.Identity.Authentication;
using AdmissionPortal.Domain.Infrastructure;
using AdmissionPortal.Infrastructure.Caching;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdmissionPortal.Infrastructure.Persistance
{
    public class AdmissionPortalDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly ICacheService _cache;

        public AdmissionPortalDbContext(DbContextOptions<AdmissionPortalDbContext> options,
                                        ICacheService cache)
            : base(options)
        {
            _cache = cache;
        }

        // Add your DbSets here
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configurations (optional)
            // modelBuilder.ApplyConfigurationsFromAssembly(typeof(AdmissionPortalDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) 
            => base.SaveChangesAsync(cancellationToken);

        public async Task<int> SaveChangesAsync(string key, CancellationToken cancellationToken = default)
        {
            int result = await base.SaveChangesAsync(cancellationToken);

            await _cache.RemoveAsync(key);

            return result;
        }
    }
}
