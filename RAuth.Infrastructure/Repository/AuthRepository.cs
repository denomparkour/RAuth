using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RAuth.Application.Constants;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Application.Repository;
using RAuth.Core.Exceptions;
using RAuth.Core.Models.OtpModel;
using RAuth.Core.Models.TokenStoreModel;
using RAuth.Core.Models.User;
using RAuth.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RAuth.Infrastructure.Repository
{
    public class AuthRepository(UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration, ApplicationDbContext db, SignInManager<ApplicationUser> signInManager) : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;
        private readonly ApplicationDbContext _db = db;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        public async Task GenerateOtp(ApplicationUser user)
        {
            Random random = new();
            int Otp = random.Next(111111, 999999);
            var existingOtp = await _db.Otp.FirstOrDefaultAsync(x => x.UserId == user.Id);
            var existingUser = await _userManager.FindByEmailAsync(user.Email!);
            if (existingUser != null && existingUser.LockoutEnabled == false)
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
            if (existingOtp == null || existingOtp.Expiry < DateTime.UtcNow)
            {
                await GenerateOtp(user);
                return GlobalConstants.OTP_NOT_FOUND;
            }
            if (verifyUser.Otp == existingOtp.Otp)
            {
                user.LockoutEnabled = false;
                await _userManager.UpdateAsync(user);
                _db.Otp.Remove(existingOtp);
                await _db.SaveChangesAsync();
                return GenerateJwtToken(user);
            }
            return GlobalConstants.INVALID_OTP;

        }

        public async Task<LoginResponseDTO> CreateUserAsync(CreateUserDTO createUser)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(createUser);
            var result = createUser.Password != null ? await _userManager.CreateAsync(user, createUser.Password) : await _userManager.CreateAsync(user);
            if (result.Succeeded)
            {
                var newUser = await _userManager.FindByEmailAsync(user.Email!) ?? throw new NotFoundException(GlobalConstants.USER_NOT_FOUND);
                if (newUser.PasswordHash == null)
                {
                    newUser.LockoutEnabled = false;
                    await _userManager.UpdateAsync(newUser);
                    LoginResponseDTO loginResponse = new()
                    {
                        JWT = GenerateJwtToken(newUser),
                        RefreshToken = await GenerateRefreshToken(newUser.Id)
                    };
                    return loginResponse;
                }
                await GenerateOtp(newUser);
                LoginResponseDTO verifyLoginResponse = new()
                {
                    JWT = GlobalConstants.VERIFY_OTP_TO_CONTINUE,
                    RefreshToken = null
                };
                return verifyLoginResponse;

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
            return new LoginResponseDTO();
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
                Expires = DateTime.UtcNow.AddMinutes(15),
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

        public async Task<LoginResponseDTO> GoogleOAuthAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                throw new NotFoundException(GlobalConstants.SIGN_IN_FAILED);
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);
            var Profile = info.Principal.FindFirst("urn:google:picture");
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                LoginResponseDTO loginResponse = new()
                {
                    JWT = GenerateJwtToken(user),
                    RefreshToken = await GenerateRefreshToken(user.Id)
                };
                return loginResponse;
            }
            CreateUserDTO createUser = new()
            {
                Email = email,
                UserName = name.Replace(" ", "").ToLower(),
                ProfilePicture = Profile.Value,
            };
            return await CreateUserAsync(createUser);
        }

        public async Task<LoginResponseDTO> LoginUserAsync(LoginUserDTO loginUser)
        {
            var existingUser = await _userManager.FindByEmailAsync(loginUser.Email) ?? throw new FailedException(GlobalConstants.INVALID_USER);
            bool isValid = await _userManager.CheckPasswordAsync(existingUser, loginUser.Password);
            if (!isValid)
            {
                throw new FailedException(GlobalConstants.INVALID_USER);
            }
            LoginResponseDTO loginResponse = new()
            {
                JWT = GenerateJwtToken(existingUser),
                RefreshToken = await GenerateRefreshToken(existingUser.Id)
            };
            return loginResponse;
        }

        public async Task<string> RefreshAsync(RefreshTokenDTO refreshToken)
        {
            var existingToken = await _db.UserTokenStore.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken.RefreshToken) ?? throw new FailedException(GlobalConstants.INVALID_REFRESH_TOKEN);
            if (existingToken.ExpiryTime < DateTime.UtcNow)
            {
                throw new FailedException(GlobalConstants.REFRESH_TOKEN_EXPIRED);
            }
            var existingUser = await _userManager.FindByIdAsync(existingToken.UserId) ?? throw new FailedException(GlobalConstants.INVALID_USER);
            return GenerateJwtToken(existingUser);
        }

        public async Task<string> GenerateRefreshToken(string UserId)
        {
            var existingUser = await _userManager.FindByIdAsync(UserId) ?? throw new FailedException(GlobalConstants.INVALID_USER);
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            byte[] randomBytes = new byte[32];
            rng.GetBytes(randomBytes);
            string refreshToken = Convert.ToBase64String(randomBytes);

            var existingToken = await _db.UserTokenStore.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (existingToken != null)
            {
                _db.UserTokenStore.Remove(existingToken);
                await _db.SaveChangesAsync();
            }
            UserTokenStore userTokenStore = new() { UserId = UserId, RefreshToken = refreshToken };
            try
            {
                await _db.UserTokenStore.AddAsync(userTokenStore);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new FailedException(ex.Message);
            }
            return refreshToken;
        }

        public async Task<string> LogoutAsync(RefreshTokenDTO refreshToken)
        {
            var existingToken = await _db.UserTokenStore.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken.RefreshToken) ?? throw new NotFoundException(GlobalConstants.INVALID_REFRESH_TOKEN);
            try
            {
                _db.UserTokenStore.Remove(existingToken);
                await _db.SaveChangesAsync();
                return GlobalConstants.SUCCESS;
            }
            catch (Exception ex)
            {
                throw new FailedException(ex.Message);
            }
        }
    }
}
