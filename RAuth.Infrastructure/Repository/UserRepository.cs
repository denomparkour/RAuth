using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RAuth.Application.Constants;
using RAuth.Application.DTO.UserDTO;
using RAuth.Application.Repository;
using RAuth.Core.Exceptions;
using RAuth.Core.Models.AddressModel;
using RAuth.Core.Models.User;
using RAuth.Infrastructure.Data;

namespace RAuth.Infrastructure.Repository
{
    public class UserRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IMapper mapper) : IUserRepository
    {
        private readonly ApplicationDbContext _db = db;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IMapper _mapper = mapper;
        public async Task<string> UpdateUserInfoAsync(string UserId, UpdateUserDTO updateUser)
        {
            var existingUser = await _userManager.FindByIdAsync(UserId) ?? throw new NotFoundException(GlobalConstants.USER_NOT_FOUND);
            existingUser.UserName = updateUser.UserName;
            existingUser.PhoneNumber = updateUser.PhoneNumber;
            existingUser.DateOfBirth = updateUser.DateOfBirth;
            existingUser.ProfilePicture = updateUser.ProfilePicture;
            var existingAddress = await _db.Address.FirstOrDefaultAsync(x => x.Id == existingUser.AddressId);
            if (existingAddress == null)
            {
                existingAddress = _mapper.Map<Address>(updateUser.Address);
                _db.Address.Add(existingAddress);
                existingUser.AddressId = existingAddress.Id;
            }
            else
            {
                _mapper.Map(updateUser.Address, existingAddress);
                _db.Address.Update(existingAddress);
            }
            try
            {
                await _db.SaveChangesAsync();
                await _userManager.UpdateAsync(existingUser);
                return GlobalConstants.SUCCESS;
            }
            catch (Exception ex)
            {
                throw new FailedException(ex.Message);
            }
        }
    }
}
