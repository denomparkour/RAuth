using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RAuth.Application.Constants;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.DTO.RAuthDTO;
using RAuth.Application.DTO.ResponseDTO;
using RAuth.Application.Repository;
using RAuth.Application.Util;
using RAuth.Core.Exceptions;
using RAuth.Core.Models.RAuthModel;
using RAuth.Core.Models.TokenStoreModel;
using RAuth.Core.Models.User;
using RAuth.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RAuth.Infrastructure.Repository
{
    public class RAuthRepository(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor, UserManager<ClientUser> userManager, UserManager<ApplicationUser> applicationUserManager, IMapper mapper, IConfiguration configuration) : IRAuthRepository
    {
        private readonly ApplicationDbContext _db = db;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly UserManager<ClientUser> _userManager = userManager;
        private readonly UserManager<ApplicationUser> _applicationUserManager = applicationUserManager;
        private readonly IMapper _mapper = mapper;
        private readonly IConfiguration _configuration = configuration;

        public string GenerateJwtToken(ClientUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"],

            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<CreateRAuthResponseDTO> CreateClientAsync(CreateRAuthDTO createRAuth)
        {
            var existingUser = await _userManager.FindByEmailAsync(createRAuth.Email);
            if (existingUser != null)
            {
                throw new FailedException(GlobalConstants.USER_ALREADY_VERIFIED);
            }
            var newClientUser = _mapper.Map<ClientUser>(createRAuth);
            var result = await _userManager.CreateAsync(newClientUser, createRAuth.Password);
            if (result.Succeeded)
            {
                ClientCredStore clientCredStore = new();
                clientCredStore.ClientId = newClientUser.Id;
                clientCredStore.ClientSecret = GenerateRAuthSecrets.GenerateClientSecret();
                await _db.ClientCredStore.AddAsync(clientCredStore);
                await _db.SaveChangesAsync();
                var response = _mapper.Map<CreateRAuthResponseDTO>(clientCredStore);
                response.RefreshToken = await GenerateRefreshToken(newClientUser.Id);
                response.JWT = GenerateJwtToken(newClientUser);
                return response;
            }
            throw new FailedException(GlobalConstants.CREATE_CLIENT_FAILED);
        }

        public async Task<string> DeleteClientAsync()
        {
            var UserId = ExtractUserId.Extract(_httpContextAccessor);
            var existingUser = await _userManager.FindByIdAsync(UserId) ?? throw new NotFoundException(GlobalConstants.USER_NOT_FOUND);
            try
            {
                await _userManager.DeleteAsync(existingUser);
                return GlobalConstants.SUCCESS;
            }
            catch (Exception ex)
            {
                throw new FailedException(ex.Message);
            }
        }

        public async Task<LoginResponseDTO> LoginClientAsync(LoginRAuthDTO loginRAuth)
        {
            var existingUser = await _userManager.FindByEmailAsync(loginRAuth.Email) ?? throw new FailedException(GlobalConstants.INVALID_USER);
            bool isValid = await _userManager.CheckPasswordAsync(existingUser, loginRAuth.Password);
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

        public async Task<string> UpdateClientAsync(UpdateRAuthDTO updateRAuth)
        {
            var UserId = ExtractUserId.Extract(_httpContextAccessor);
            var existingUser = await _userManager.FindByIdAsync(UserId) ?? throw new NotFoundException(GlobalConstants.USER_NOT_FOUND);
            existingUser.PhoneNumber = updateRAuth.PhoneNumber;
            existingUser.OrganizationName = updateRAuth.OrganizationName;
            existingUser.UserName = updateRAuth.OrganizationUserName;
            existingUser.ProfilePicture = updateRAuth.ProfilePictureUrl;
            try
            {
                await _userManager.UpdateAsync(existingUser);
                return GlobalConstants.SUCCESS;
            }
            catch (Exception ex)
            {
                throw new FailedException(ex.Message);
            }
        }

        public async Task<string> VerifyClientAsync(VerifyClientDTO verifyClient)
        {
            var userId = ExtractUserId.Extract(_httpContextAccessor);
            if (verifyClient.ClientId != userId)
            {
                throw new FailedException(GlobalConstants.INVALID_USER);
            }
            var existingUser = await _userManager.FindByIdAsync(userId);
            if (existingUser == null)
            {
                throw new FailedException(GlobalConstants.USER_NOT_FOUND);
            }
            var existingClientInfo = await _db.ClientCredStore.FirstOrDefaultAsync(x => x.ClientId == userId) ?? throw new FailedException(GlobalConstants.EXISTING_CLIENT_NOT_FOUND);
            if (existingClientInfo.ClientSecret == verifyClient.ClientSecret)
            {
                return GlobalConstants.SUCCESS;
            }
            return GlobalConstants.FAILED;
        }

        public async Task<string> RefreshAsync(RefreshTokenDTO refreshToken)
        {
            var existingToken = await _db.ClientTokenStore.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken.RefreshToken) ?? throw new FailedException(GlobalConstants.INVALID_REFRESH_TOKEN);
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

            var existingToken = await _db.ClientTokenStore.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (existingToken != null)
            {
                _db.ClientTokenStore.Remove(existingToken);
                await _db.SaveChangesAsync();
            }
            ClientTokenStore ClientTokenStore = new() { UserId = UserId, RefreshToken = refreshToken };
            try
            {
                await _db.ClientTokenStore.AddAsync(ClientTokenStore);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new FailedException(ex.Message);
            }
            return refreshToken;
        }
        public async Task<string> LogoutAsync()
        {
            var UserId = ExtractUserId.Extract(_httpContextAccessor);
            var existingToken = await _db.ClientTokenStore.FirstOrDefaultAsync(x => x.UserId == UserId) ?? throw new NotFoundException(GlobalConstants.INVALID_REFRESH_TOKEN);
            try
            {
                _db.ClientTokenStore.Remove(existingToken);
                await _db.SaveChangesAsync();
                return GlobalConstants.SUCCESS;
            }
            catch (Exception ex)
            {
                throw new FailedException(ex.Message);
            }
        }

        public async Task<GetRAuthUserResponseDTO> GetRAuthUserAsync(GetRAuthUserDTO getRAuthUser)
        {
            var existingUser = await _applicationUserManager.FindByNameAsync(getRAuthUser.UserName) ?? throw new NotFoundException(GlobalConstants.USER_NOT_FOUND);
            var address = await _db.Address.FirstOrDefaultAsync(x => x.Id == existingUser.AddressId);
            GetRAuthUserResponseDTO getRAuthUserResponseDTO = new() { Address = address, DateOfBirth = existingUser.DateOfBirth, Email = existingUser.Email, PhoneNumber = existingUser.PhoneNumber, ProfilePicture = existingUser.ProfilePicture, UserName = existingUser.UserName };
            return getRAuthUserResponseDTO;
        }
    }
}
