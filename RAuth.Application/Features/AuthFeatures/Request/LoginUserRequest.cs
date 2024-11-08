using MediatR;
using RAuth.Application.DTO.AuthDTO;

namespace RAuth.Application.Features.AuthFeatures.Request
{
    public class LoginUserRequest : IRequest<string>
    {
        public LoginUserDTO LoginUser { get; set; }
    }
}
