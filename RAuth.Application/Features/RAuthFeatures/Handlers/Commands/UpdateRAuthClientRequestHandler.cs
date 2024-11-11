using MediatR;
using RAuth.Application.Features.RAuthFeatures.Requests;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.RAuthFeatures.Handlers.Commands
{
    public class UpdateRAuthClientRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateRAuthClientRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public Task<string> Handle(UpdateRAuthClientRequest request, CancellationToken cancellationToken)
        {
            return _unitOfWork.RAuthRepository.UpdateClientAsync(request.updateRAuth);
        }
    }
}
