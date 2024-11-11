using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.DTO.RAuthDTO;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Application.Features.RAuthFeatures.Requests;

namespace RAuth.API.Controllers
{
    [ApiController]
    [Route("rauth")]
    public class RAuthController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("create")]
        public async Task<IActionResult> CreateClientAsync([FromBody] CreateRAuthDTO createRAuth)
        {
            var result = await _mediator.Send(new CreateRAuthClientRequest { createRAuth = createRAuth });
            return Ok(ResponseBuilder.Build(result));
        }
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateClientAsync([FromBody] UpdateRAuthDTO updateRAuth)
        {
            var result = await _mediator.Send(new UpdateRAuthClientRequest { updateRAuth = updateRAuth });
            return Ok(ResponseBuilder.Build(result));
        }
        [Authorize]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteClientAsync()
        {
            var result = await _mediator.Send(new DeleteRAuthClientRequest());
            return Ok(ResponseBuilder.Build(result));
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginClientAsync([FromBody] LoginRAuthDTO loginRAuth)
        {
            var result = await _mediator.Send(new RAuthClientLoginRequest { RAuthClientLogin = loginRAuth });
            return Ok(ResponseBuilder.Build(result));
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDTO refreshToken)
        {
            var result = await _mediator.Send(new RefreshRAuthClientRequest { RefreshToken = refreshToken });
            return Ok(ResponseBuilder.Build(result));
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            var result = await _mediator.Send(new LogoutRAuthClientRequest());
            return Ok(ResponseBuilder.Build(result));
        }
        [Authorize]
        [HttpGet("get")]
        public async Task<IActionResult> GetAsync([FromQuery] GetRAuthUserDTO getRAuthUser)
        {
            var result = await _mediator.Send(new GetRAuthClientRequest { GetRAuthUserDTO = getRAuthUser });
            return Ok(ResponseBuilder.Build(result));
        }
    }
}
