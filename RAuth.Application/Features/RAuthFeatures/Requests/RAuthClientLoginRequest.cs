using MediatR;
using RAuth.Application.DTO.RAuthDTO;
using RAuth.Application.DTO.ResponseDTO;

namespace RAuth.Application.Features.RAuthFeatures.Requests
{
    public class RAuthClientLoginRequest : IRequest<LoginResponseDTO>
    {
        public LoginRAuthDTO RAuthClientLogin { get; set; }
    }
}
