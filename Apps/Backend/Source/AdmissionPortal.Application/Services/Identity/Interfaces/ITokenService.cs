using AdmissionPortal.Application.DTOs.Identity;
using AdmissionPortal.Domain.Abstractions;
using AdmissionPortal.Domain.Entities.Identity;
using System.Security.Claims;

namespace AdmissionPortal.Application.Services.Identity.Interfaces
{
    public interface ITokenService : IDomainService
    {
        Task<AuthenticationResponseDto> GenerateTokensAsync(ApplicationUser user, CancellationToken cancellationToken = default);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string accessToken, string refreshToken);
    }
}