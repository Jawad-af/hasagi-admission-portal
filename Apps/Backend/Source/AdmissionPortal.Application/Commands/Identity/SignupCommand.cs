using AdmissionPortal.Application.DTOs.Identity;
using AdmissionPortal.Application.Services.Identity.Interfaces;
using Microsoft.Extensions.Logging;

namespace AdmissionPortal.Application.Commands.Identity
{
    public class SignupCommand
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class SignupCommandHandler
    {
        private readonly IIdentityService _identityService;
        private readonly ILogger<SignupCommandHandler> _logger;

        public SignupCommandHandler(ILogger<SignupCommandHandler> logger,
                                    IIdentityService identityService)
        {
            _logger = logger;
            _identityService = identityService;
        }

        public async Task<AuthenticationResponseDto> Handle(SignupCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Started signup process for user: {FullName}", request.FullName);

            AuthenticationResponseDto response = await _identityService.Signup(request, cancellationToken);

            _logger.LogInformation("Started signup process for user: {FullName}", request.FullName);

            return response;
        }
    }
}
