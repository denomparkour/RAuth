using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RAuth.Application.Constants;
using RAuth.Application.DTO.AuthDTO;
using RAuth.Application.Repository;
using RAuth.Core.Exceptions;
using RAuth.Core.Models.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RAuth.Infrastructure.Repository
{
    public class AuthRepository(UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration) : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMapper _mapper = mapper;
        public async Task<string> CreateUserAsync(CreateUserDTO createUser)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(createUser);
            var result = await _userManager.CreateAsync(user, createUser.Password);
            if (result.Succeeded)
            {
                var newUser = await _userManager.FindByEmailAsync(user.Email!);
                return GenerateJwtToken(newUser!);
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
            Console.WriteLine("Came here for token");
            return tokenHandler.WriteToken(token);
        }
    }
}
