using MediatR;
using RAuth.Application.Features.AuthFeatures.Request;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.AuthFeatures.Handlers.Queries
{
    public class LoginUserRequestHandler(IAuthRepository authRepository) : IRequestHandler<LoginUserRequest, string>
    {
        private readonly IAuthRepository _authRepository = authRepository;
        public Task<string> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            return _authRepository.LoginUserAsync(request.LoginUser);
        }
    }
}
