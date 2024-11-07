using MediatR;
using RAuth.Application.Features.AuthFeatures.Request;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.AuthFeatures.Handlers.Commands
{
    public class CreateUserRequestHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserRequest, string>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<string> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.AuthRepository.CreateUserAsync(request.createUser);
            return result;
        }
    }
}
