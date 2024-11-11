using MediatR;
using RAuth.Application.DTO.RAuthDTO;
using RAuth.Application.Features.RAuthFeatures.Requests;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.RAuthFeatures.Handlers.Commands
{
    public class CreateRAuthClientRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateRAuthClientRequest, CreateRAuthResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<CreateRAuthResponseDTO> Handle(CreateRAuthClientRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.RAuthRepository.CreateClientAsync(request.createRAuth);
        }
    }
}
