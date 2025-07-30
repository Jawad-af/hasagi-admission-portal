using AdmissionPortal.Application.Commands.Identity;
using AdmissionPortal.Application.DTOs.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<AuthenticationResponseDto>> Signup([FromBody] SignupCommand command)
        {
            AuthenticationResponseDto response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPost("identity/login")]
        public async Task<ActionResult<AuthenticationResponseDto>> Login([FromBody] LoginCommand command) 
        {            
            AuthenticationResponseDto response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPost("identity/refresh-token")]
        public async Task<ActionResult<AuthenticationResponseDto>> Refresh([FromBody] RefreshTokenCommand command)
        {
            AuthenticationResponseDto response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}