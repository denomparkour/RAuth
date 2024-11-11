using Microsoft.AspNetCore.Identity;
using RAuth.Application.Constants;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.DTO.RAuthDTO;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Core.Exceptions;

namespace RAuth.Application.Repository
{
    public interface IRAuthRepository
    {
        Task<string> VerifyClientAsync(VerifyClientDTO verifyClient);
        Task<CreateRAuthResponseDTO> CreateClientAsync(CreateRAuthDTO createRAuth);
        Task<string> UpdateClientAsync(UpdateRAuthDTO updateRAuth);
        Task<string> DeleteClientAsync();
        Task<LoginResponseDTO> LoginClientAsync(LoginRAuthDTO loginRAuth);
        Task<string> RefreshAsync(RefreshTokenDTO refreshToken);
        Task<string> GenerateRefreshToken(string UserId);
        Task<string> LogoutAsync();
        Task<GetRAuthUserResponseDTO> GetRAuthUserAsync(GetRAuthUserDTO getRAuthUser);
    }
}
