using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Core.Models.User;

namespace RAuth.Application.Repository
{
    public interface IAuthRepository
    {
        string GenerateJwtToken(ApplicationUser user);
        Task<LoginResponseDTO> CreateUserAsync(CreateUserDTO createUser);
        Task<string> VerifyUserAsync(VerifyUserDTO verifyUser);
        Task<LoginResponseDTO> LoginUserAsync(LoginUserDTO loginUser);
        Task<string> RefreshAsync(RefreshTokenDTO refreshToken);
        Task GenerateOtp(ApplicationUser user);
        Task<string> VerifyOtp(VerifyUserDTO verifyUser);
        Task<LoginResponseDTO> GoogleOAuthAsync();
        Task<string> GenerateRefreshToken(string UserId);
        Task<string> LogoutAsync(RefreshTokenDTO refreshToken);
    }
}
