using MediatR;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Application.Features.AuthFeatures.Request;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.AuthFeatures.Handlers.Queries
{
    public class OAuthRequestHandler(IAuthRepository authRepository) : IRequestHandler<OAuthRequest, LoginResponseDTO>
    {
        private readonly IAuthRepository _authRepository = authRepository;
        public async Task<LoginResponseDTO> Handle(OAuthRequest request, CancellationToken cancellationToken)
        {
            return await _authRepository.GoogleOAuthAsync();
        }
    }
}
