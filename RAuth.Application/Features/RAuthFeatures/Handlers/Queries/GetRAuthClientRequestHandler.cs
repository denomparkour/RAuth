using MediatR;
using RAuth.Application.DTO.RAuthDTO;
using RAuth.Application.Features.RAuthFeatures.Requests;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.RAuthFeatures.Handlers.Queries
{
    public class GetRAuthClientRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRAuthClientRequest, GetRAuthUserResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<GetRAuthUserResponseDTO> Handle(GetRAuthClientRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.RAuthRepository.GetRAuthUserAsync(request.GetRAuthUserDTO);
        }
    }
}
