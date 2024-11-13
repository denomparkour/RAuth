using MediatR;
using RAuth.Application.DTO.RAuthDTO;

namespace RAuth.Application.Features.RAuthFeatures.Requests
{
    public class GetRAuthUserRequest : IRequest<GetRAuthUserResponseDTO>
    {
        public GetRAuthUserDTO GetRAuthUserDTO {  get; set; }
    }
}
