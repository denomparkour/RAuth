using MediatR;
using RAuth.Application.DTO.RAuthDTO;

namespace RAuth.Application.Features.RAuthFeatures.Requests
{
    public class UpdateRAuthClientRequest : IRequest<string>
    {
        public UpdateRAuthDTO updateRAuth { get; set; }
    }
}
