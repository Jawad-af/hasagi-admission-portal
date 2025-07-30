using AdmissionPortal.Application.DTOs.Identity;
using AdmissionPortal.Domain.Abstractions;
using AdmissionPortal.Domain.Entities.Identity;

namespace AdmissionPortal.Application.Services.Identity.Interfaces
{
    public interface ITokenService : IDomainService
    {
        Task<AuthenticationResponseDto> GenerateTokensAsync(ApplicationUser user, CancellationToken cancellationToken = default);
        string GenerateJwtToken(ApplicationUser user);
        RefreshToken GenerateRefreshToken(string userId);
    }
}