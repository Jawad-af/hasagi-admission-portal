using AdmissionPortal.Application.Commands.Identity;
using AdmissionPortal.Application.DTOs.Identity;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace AdmissionPortal.WebApi.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IMessageBus _bus;

        public IdentityController(IMessageBus bus)
        {
            _bus = bus;
        }

        [HttpPost("identity/signup")]
        public async Task<ActionResult<AuthenticationResponseDto>> Signup([FromBody] SignupCommand command,
                                                                           CancellationToken cancellationToken = default)
        {
            AuthenticationResponseDto response = await _bus.InvokeAsync<AuthenticationResponseDto>(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost("identity/login")]
        public async Task<ActionResult<AuthenticationResponseDto>> Login([FromBody] LoginCommand command,
                                                                          CancellationToken cancellationToken = default) 
        {            
            AuthenticationResponseDto response = await _bus.InvokeAsync<AuthenticationResponseDto>(command, cancellationToken);

            return Ok(response);
        }

        [HttpPost("identity/refresh-token")]
        public async Task<ActionResult<AuthenticationResponseDto>> Refresh([FromBody] RefreshTokenCommand command,
                                                                            CancellationToken cancellationToken = default)
        {
            AuthenticationResponseDto response = await _bus.InvokeAsync<AuthenticationResponseDto>(command, cancellationToken);

            return Ok(response);
        }
    }
}