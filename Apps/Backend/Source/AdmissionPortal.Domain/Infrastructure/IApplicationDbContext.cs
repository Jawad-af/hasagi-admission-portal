using AdmissionPortal.Domain.Entities.Identity.Authentication;
using Microsoft.EntityFrameworkCore;

namespace AdmissionPortal.Domain.Infrastructure
{
    public interface IApplicationDbContext
    {
        DbSet<RefreshToken> RefreshTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(string key, CancellationToken cancellationToken = default);
    }
}
