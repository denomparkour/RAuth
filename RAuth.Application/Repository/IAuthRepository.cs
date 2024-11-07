using RAuth.Application.DTO.AuthDTO;
using RAuth.Core.Models.User;

namespace RAuth.Application.Repository
{
    public interface IAuthRepository
    {
        string GenerateJwtToken(ApplicationUser user);
        Task<string> CreateUserAsync(CreateUserDTO createUser);
    }
}
