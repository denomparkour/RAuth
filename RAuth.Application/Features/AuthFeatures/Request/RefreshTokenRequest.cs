using MediatR;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.DTO.ResponseDTO;

namespace RAuth.Application.Features.AuthFeatures.Request
{
    public class RefreshTokenRequest : IRequest<string>
    {
        public RefreshTokenDTO RefreshToken { get; set; }
    }
}
