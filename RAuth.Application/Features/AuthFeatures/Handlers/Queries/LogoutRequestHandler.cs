using MediatR;
using RAuth.Application.Features.AuthFeatures.Request;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.AuthFeatures.Handlers.Queries
{
    public class LogoutRequestHandler(IAuthRepository authRepository) : IRequestHandler<LogoutUserRequest, string>
    {
        private readonly IAuthRepository _authRepository = authRepository;
        public async Task<string> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
        {
            return await _authRepository.LogoutAsync(request.RefreshToken);
        }
    }
}
