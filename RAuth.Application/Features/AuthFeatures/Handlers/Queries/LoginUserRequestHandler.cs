using MediatR;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Application.Features.AuthFeatures.Request;
using RAuth.Application.Repository;

namespace RAuth.Application.Features.AuthFeatures.Handlers.Queries
{
    public class LoginUserRequestHandler(IAuthRepository authRepository) : IRequestHandler<LoginUserRequest, LoginResponseDTO>
    {
        private readonly IAuthRepository _authRepository = authRepository;
        public Task<LoginResponseDTO> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            return _authRepository.LoginUserAsync(request.LoginUser);
        }
    }
}
