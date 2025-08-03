using AdmissionPortal.Domain.Abstractions;
using AdmissionPortal.Domain.Entities.Identity.Authentication;

namespace AdmissionPortal.Application.Validations.Identity.Interfaces
{
    public interface IRefreshTokenValidation : IDomainService
    {
        Task<RefreshToken> Validate_GetRefreshToken(string refreshToken, string userId, CancellationToken cancellationToken = default);
    }
}
