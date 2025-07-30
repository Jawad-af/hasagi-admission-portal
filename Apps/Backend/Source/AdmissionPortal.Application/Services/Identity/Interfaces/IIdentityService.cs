using AdmissionPortal.Application.Commands.Identity;
using AdmissionPortal.Application.DTOs.Identity;
using AdmissionPortal.Domain.Abstractions;

namespace AdmissionPortal.Application.Services.Identity.Interfaces
{
    public interface IIdentityService : IDomainService
    {
        Task<AuthenticationResponseDto> Login(LoginCommand command, CancellationToken cancellationToken);
        Task<AuthenticationResponseDto> Signup(SignupCommand command, CancellationToken cancellationToken);
        Task<AuthenticationResponseDto> RefreshToken(RefreshTokenCommand command, CancellationToken cancellationToken);
    }
}
