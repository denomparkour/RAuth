using MediatR;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.DTO.ResponseDTO;

namespace RAuth.Application.Features.AuthFeatures.Request
{
    public class LoginUserRequest : IRequest<LoginResponseDTO>
    {
        public LoginUserDTO LoginUser { get; set; }
    }
}
