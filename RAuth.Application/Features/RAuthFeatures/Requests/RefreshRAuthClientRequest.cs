using MediatR;
using RAuth.Application.DTO.AuthDTO;

namespace RAuth.Application.Features.RAuthFeatures.Requests
{
    public class RefreshRAuthClientRequest : IRequest<string>
    {
        public RefreshTokenDTO RefreshToken { get; set; }
    }
}
