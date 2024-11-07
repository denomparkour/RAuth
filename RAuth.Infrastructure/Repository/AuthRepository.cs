using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RAuth.Application.Constants;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.Repository;
using RAuth.Core.Exceptions;
using RAuth.Core.Models.OtpModel;
using RAuth.Core.Models.User;
using RAuth.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RAuth.Infrastructure.Repository
{
    public class AuthRepository(UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration, ApplicationDbContext db) : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;
        private readonly ApplicationDbContext _db = db;

        public async Task GenerateOtp(ApplicationUser user)
        {
            Random random = new();
            int Otp = random.Next(111111, 999999);
            var existingOtp = await _db.Otp.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var existingUser = await _userManager.FindByEmailAsync(user.Email!);
            if(existingUser != null && existingUser.LockoutEnabled == false)
            {
                throw new FailedException(GlobalConstants.USER_ALREADY_VERIFIED);
            }

            if (existingOtp != null)
            {
                if (existingOtp.Expiry > DateTime.UtcNow)
                {
                    return;
                }
                _db.Otp.Remove(existingOtp);
                await _db.SaveChangesAsync();
            }
            var OtpData = new OTP()
            {
                Otp = Otp,
                UserId = user.Id
            };
            try
            {
                await _db.Otp.AddAsync(OtpData);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.StackTrace);
            }
        }

        public async Task<string> VerifyOtp(VerifyUserDTO verifyUser)
        {
            var user = await _userManager.FindByEmailAsync(verifyUser.Email) ?? throw new NotFoundException(GlobalConstants.USER_NOT_FOUND);
            var existingOtp = await _db.Otp.FirstOrDefaultAsync(x => x.UserId == user.Id);
            if(existingOtp == null || existingOtp.Expiry < DateTime.UtcNow)
            {
                await GenerateOtp(user);
                return GlobalConstants.OTP_NOT_FOUND;
            }
            if(verifyUser.Otp == existingOtp.Otp)
            {
                user.LockoutEnabled = false;
                await _userManager.UpdateAsync(user);
                _db.Otp.Remove(existingOtp);
                await _db.SaveChangesAsync();
                return GenerateJwtToken(user);
            }
            return GlobalConstants.INVALID_OTP;
            
        }

        public async Task<string> CreateUserAsync(CreateUserDTO createUser)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(createUser);
            var result = await _userManager.CreateAsync(user, createUser.Password);
            if (result.Succeeded)
            {
                var newUser = await _userManager.FindByEmailAsync(user.Email!) ?? throw new NotFoundException(GlobalConstants.USER_NOT_FOUND);
                await GenerateOtp(newUser);
                return GlobalConstants.VERIFY_OTP_TO_CONTINUE;

            }
            if (result.Errors != null)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += error.Description + "\n";
                }
                throw new CreateUserFailedException(errors);
            }
             return GlobalConstants.FAILED;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"],

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> VerifyUserAsync(VerifyUserDTO verifyUser)
        {
            return await VerifyOtp(verifyUser);
        }
    }
}
