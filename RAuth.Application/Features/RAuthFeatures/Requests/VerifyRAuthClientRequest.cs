using MediatR;
using RAuth.Application.DTO.RAuthDTO;

namespace RAuth.Application.Features.RAuthFeatures.Requests
{
    public class VerifyRAuthClientRequest : IRequest<string>
    {
        public VerifyClientDTO verifyClient { get; set; }
    }
}
