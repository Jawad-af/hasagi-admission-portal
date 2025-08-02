using AdmissionPortal.Application.Commands.Identity;
using AdmissionPortal.Application.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;
using Ultimate.Mediator;

namespace AdmissionPortal.WebApi.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public IdentityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("identity/signup")]
        public async Task<ActionResult<AuthenticationResponseDto>> Signup([FromBody] SignupCommand command,
                                                                           CancellationToken cancellationToken = default)
        {
            AuthenticationResponseDto response = await _mediator.SendCommandAsync<SignupCommand, AuthenticationResponseDto>(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost("identity/login")]
        public async Task<ActionResult<AuthenticationResponseDto>> Login([FromBody] LoginCommand command,
                                                                          CancellationToken cancellationToken = default) 
        {            
            AuthenticationResponseDto response = await _mediator.SendCommandAsync<LoginCommand, AuthenticationResponseDto>(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost("identity/refresh-token")]
        public async Task<ActionResult<AuthenticationResponseDto>> Refresh([FromBody] RefreshTokenCommand command,
                                                                            CancellationToken cancellationToken = default)
        {
            AuthenticationResponseDto response = await _mediator.SendCommandAsync<RefreshTokenCommand, AuthenticationResponseDto>(command, cancellationToken);

            return Ok(response);
        }
    }
}