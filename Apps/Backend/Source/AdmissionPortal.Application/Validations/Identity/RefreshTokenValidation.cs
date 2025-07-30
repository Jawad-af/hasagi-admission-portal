using AdmissionPortal.Application.Validations.Identity.Interfaces;
using AdmissionPortal.Domain.Entities.Identity;
using AdmissionPortal.Domain.Exceptions;
using AdmissionPortal.Domain.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AdmissionPortal.Application.Validations.Identity
{
    public class RefreshTokenValidation : IRefreshTokenValidation
    {
        private readonly IApplicationDbContext _context;

        public RefreshTokenValidation(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken> Validate_GetRefreshToken(string refreshToken, string userId, CancellationToken cancellationToken = default)
        {
            RefreshToken? token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken && rt.UserId == userId && !rt.IsRevoked, cancellationToken: cancellationToken);

            if (token == null || token.ExpiresAt < DateTime.UtcNow)
            {
                AdmissionPortalExceptions.ThrowUnAuthorizedException("Invalid or expired refresh token");
            }

            return token!;
        }
    }
}
