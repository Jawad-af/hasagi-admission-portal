using AdmissionPortal.Application.DTOs.Identity;
using AdmissionPortal.Application.Services.Identity.Interfaces;
using Microsoft.Extensions.Logging;
using Ultimate.Mediator.Interfaces;

namespace AdmissionPortal.Application.Commands.Identity
{
    public class RefreshTokenCommand : ICommand<AuthenticationResponseDto>
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }

    public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, AuthenticationResponseDto>
    {
        private readonly ILogger<RefreshTokenCommandHandler> _logger;
        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(ILogger<RefreshTokenCommandHandler> logger,
                                          IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        public async Task<AuthenticationResponseDto> HandleAsync(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started refreshing token");

            AuthenticationResponseDto response = await _identityService.RefreshToken(request, cancellationToken);

            return response;
        }
    }
}
