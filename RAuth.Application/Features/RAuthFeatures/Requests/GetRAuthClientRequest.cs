using MediatR;
using RAuth.Application.DTO.RAuthDTO;

namespace RAuth.Application.Features.RAuthFeatures.Requests
{
    public class GetRAuthClientRequest : IRequest<GetRAuthUserResponseDTO>
    {
        public GetRAuthUserDTO GetRAuthUserDTO { get; set; }
    }
}
