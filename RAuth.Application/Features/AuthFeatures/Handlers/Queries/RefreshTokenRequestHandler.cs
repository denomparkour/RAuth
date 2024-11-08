using MediatR;
using RAuth.Application.Features.AuthFeatures.Request;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.AuthFeatures.Handlers.Queries
{
    public class RefreshTokenRequestHandler(IAuthRepository authRepository) : IRequestHandler<RefreshTokenRequest, string>
    {
        private readonly IAuthRepository _authRepository = authRepository;
        public Task<string> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            return _authRepository.RefreshAsync(request.RefreshToken);
        }
    }
}
