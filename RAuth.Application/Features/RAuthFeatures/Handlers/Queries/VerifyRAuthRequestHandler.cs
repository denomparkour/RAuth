using MediatR;
using RAuth.Application.Features.RAuthFeatures.Requests;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.RAuthFeatures.Handlers.Queries
{
    public class VerifyRAuthRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<VerifyRAuthClientRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<string> Handle(VerifyRAuthClientRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.RAuthRepository.VerifyClientAsync(request.verifyClient);
        }
    }
}
