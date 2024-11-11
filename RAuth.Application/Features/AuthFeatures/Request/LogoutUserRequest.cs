using MediatR;
using RAuth.Application.DTO.AuthDTO;

namespace RAuth.Application.Features.AuthFeatures.Request
{
    public class LogoutUserRequest : IRequest<string>
    {
        public RefreshTokenDTO RefreshToken { get; set; }
    }
}
