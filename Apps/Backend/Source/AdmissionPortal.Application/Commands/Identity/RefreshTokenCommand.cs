using AdmissionPortal.Application.DTOs.Identity;
using AdmissionPortal.Application.Services.Identity.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AdmissionPortal.Application.Commands.Identity
{
    public class RefreshTokenCommand : IRequest<AuthenticationResponseDto>
    {
        public string RefreshToken { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResponseDto>
    {
        private readonly ILogger<RefreshTokenCommandHandler> _logger;
        private readonly IIdentityService _identityService;

        public RefreshTokenCommandHandler(ILogger<RefreshTokenCommandHandler> logger,
                                          IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        public async Task<AuthenticationResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started refreshing token for user : {UserId}", request.UserId);

            AuthenticationResponseDto response = await _identityService.RefreshToken(request, cancellationToken);

            return response;
        }
    }
}
