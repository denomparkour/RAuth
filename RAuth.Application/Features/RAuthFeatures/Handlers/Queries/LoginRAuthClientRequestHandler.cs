using MediatR;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Application.Features.RAuthFeatures.Requests;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.RAuthFeatures.Handlers.Queries
{
    public class LoginRAuthClientRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<RAuthClientLoginRequest, LoginResponseDTO>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<LoginResponseDTO> Handle(RAuthClientLoginRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.RAuthRepository.LoginClientAsync(request.RAuthClientLogin);
        }
    }

}
