using MediatR;
using RAuth.Application.DTO.RAuthDTO;

namespace RAuth.Application.Features.RAuthFeatures.Requests
{
    public class CreateRAuthClientRequest : IRequest<CreateRAuthResponseDTO>
    {
        public CreateRAuthDTO createRAuth { get; set; }
    }
}
