using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Application.Features.AuthFeatures.Request;
using RAuth.Core.Models.User;

namespace RAuth.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;
        [HttpPost("create")]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDTO createUser)
        {
            var result = await _mediator.Send(new CreateUserRequest { createUser = createUser });
            return Ok(ResponseBuilder.Build(result));
        }
        [HttpPost("verify")]
        public async Task<IActionResult> VerifyUserAsync([FromBody] VerifyUserDTO verifyUser)
        {
            var result = await _mediator.Send(new VerifyUserRequest { verifyUser = verifyUser });
            return Ok(ResponseBuilder.Build(result));
        }
        [HttpGet("login/google")]
        public async Task<IActionResult> LoginWithGoogle()
        {
            var query = await _mediator.Send(new OAuthLoginRequest());
            return Challenge(query, "google");
        }
        [HttpGet("login/google/handler")]
        public async Task<IActionResult> LoginWithGoogleHandler()
        {
            var query = await _mediator.Send(new OAuthRequest());
            return Ok(ResponseBuilder.Build(query));
        }
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserDTO loginUser)
        {
            var query = await _mediator.Send(new LoginUserRequest { LoginUser = loginUser });
            return Ok(ResponseBuilder.Build(query));
        }
    }
}
