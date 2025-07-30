using AdmissionPortal.Application.Commands.Identity;
using AdmissionPortal.Application.DTOs.Identity;
using AdmissionPortal.Application.Services.Identity.Interfaces;
using AdmissionPortal.Application.Validations.Identity.Interfaces;
using AdmissionPortal.Domain.Entities.Identity;
using AdmissionPortal.Domain.Infrastructure;

namespace AdmissionPortal.Application.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly ITokenService _tokenService;
        private readonly IApplicationDbContext _context;
        private readonly IRefreshTokenValidation _refreshTokenValidation;
        private readonly IIdentityManagerValidation _userManagerValidation;

        public IdentityService(ITokenService tokenService,
                               IApplicationDbContext context,
                               IRefreshTokenValidation refreshTokenValidation,
                               IIdentityManagerValidation userManagerValidation)
        {
            _tokenService = tokenService;
            _context = context;
            _refreshTokenValidation = refreshTokenValidation;
            _userManagerValidation = userManagerValidation;
        }

        public async Task<AuthenticationResponseDto> Login(LoginCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _userManagerValidation.Validate_GetUserByEmail(command.Email, cancellationToken);

            await _userManagerValidation.Validate_LoginUser(user, command.Password, cancellationToken);
            
            AuthenticationResponseDto response = await _tokenService.GenerateTokensAsync(user, cancellationToken);

            return response;
        }

        public async Task<AuthenticationResponseDto> RefreshToken(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _userManagerValidation.Validate_GetUserById(command.UserId, cancellationToken);

            RefreshToken token = await _refreshTokenValidation.Validate_GetRefreshToken(command.RefreshToken, command.UserId, cancellationToken);

            token!.IsRevoked = true;

            await _context.SaveChangesAsync(cancellationToken);

            AuthenticationResponseDto response = await _tokenService.GenerateTokensAsync(user, cancellationToken);

            return response;
        }

        public async Task<AuthenticationResponseDto> Signup(SignupCommand command, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser
            {
                UserName = command.Email,
                Email = command.Email,
                FullName = command.FullName
            };

            await _userManagerValidation.Validate_CreateUser(user, command.Password, cancellationToken);

            AuthenticationResponseDto response = await _tokenService.GenerateTokensAsync(user, cancellationToken);

            return response;
        }
    }
}
