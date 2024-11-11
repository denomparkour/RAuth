using MediatR;
using RAuth.Application.Features.RAuthFeatures.Requests;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.RAuthFeatures.Handlers.Commands
{
    public class RefreshRAuthClientRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<RefreshRAuthClientRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public Task<string> Handle(RefreshRAuthClientRequest request, CancellationToken cancellationToken)
        {
            return _unitOfWork.RAuthRepository.RefreshAsync(request.RefreshToken);
        }
    }
}
