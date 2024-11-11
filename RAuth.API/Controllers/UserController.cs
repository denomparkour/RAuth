using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Application.DTO.UserDTO;
using RAuth.Application.Features.UserFeatures.Request;
using System.Security.Claims;

namespace RAuth.API.Controllers
{
    [ApiController]
    [Route("user")]
    [Authorize]
    public class UserController(IMediator mediator) : Controller
    {
        private readonly IMediator _mediator = mediator;
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDTO updateUser)
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            var query = await _mediator.Send(new UpdateUserRequest { UserId = userId, updateUser = updateUser });
            return Ok(ResponseBuilder.Build(query));
        }
    }
}
