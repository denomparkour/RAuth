using MediatR;
using RAuth.Application.Features.UserFeatures.Request;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.UserFeatures.Handlers.Commands
{
    public class UpdateUserRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<string> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.UserRepository.UpdateUserInfoAsync(request.UserId, request.updateUser);
        }
    }
}
