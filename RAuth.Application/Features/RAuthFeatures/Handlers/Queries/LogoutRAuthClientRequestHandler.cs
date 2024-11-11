using MediatR;
using RAuth.Application.Features.RAuthFeatures.Requests;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.RAuthFeatures.Handlers.Queries
{
    public class LogoutRAuthClientRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<LogoutRAuthClientRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<string> Handle(LogoutRAuthClientRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.RAuthRepository.LogoutAsync();
        }
    }
}
