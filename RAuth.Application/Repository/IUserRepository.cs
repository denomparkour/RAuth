using RAuth.Application.DTO.UserDTO;

namespace RAuth.Application.Repository
{
    public interface IUserRepository
    {
        Task<string> UpdateUserInfoAsync(string UserId, UpdateUserDTO updateUser);
    }
}
