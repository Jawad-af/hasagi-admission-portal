using AdmissionPortal.Application.DTOs.Identity;
using AdmissionPortal.Application.Services.Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace AdmissionPortal.Application.Commands.Identity
{
    public class LoginCommand
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class LoginCommandHandler
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(ILogger<LoginCommandHandler> logger,
                                   IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        public async Task<AuthenticationResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started logging in user with email: {Email}", request.Email);

            AuthenticationResponseDto response = await _identityService.Login(request, cancellationToken);

            _logger.LogInformation("Finished logging in user with email: {Email}", request.Email);

            return response;
        }
    }
}
