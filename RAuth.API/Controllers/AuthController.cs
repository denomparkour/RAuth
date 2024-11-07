using MediatR;
using Microsoft.AspNetCore.Mvc;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Application.Features.AuthFeatures.Request;

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
    }
}
