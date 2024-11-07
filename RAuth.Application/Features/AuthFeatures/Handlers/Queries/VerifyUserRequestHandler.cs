using MediatR;
using RAuth.Application.Features.AuthFeatures.Request;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.AuthFeatures.Handlers.Queries
{
    public class VerifyUserRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<VerifyUserRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<string> Handle(VerifyUserRequest request, CancellationToken cancellationToken)
        {
           return await _unitOfWork.AuthRepository.VerifyUserAsync(request.verifyUser);
        }
    }
}
