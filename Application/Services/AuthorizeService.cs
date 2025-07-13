using Application.Configuration;
using Application.Features.AccountContext.Dto;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using FluentValidation;

namespace Application.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AuthorizeService(UserManager<User> userManager, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value ?? throw new ArgumentException("JWT settings are missing in configuration.");
        }

        public async Task<string> GenerateJwtTokenAsync(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName ?? "Unknown")
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiryHours),
                SigningCredentials = creds,
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<User> ValidateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new UnauthorizedAccessException("Error: Phone number or password cannot be empty.");
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username ||u.Email==username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Error: User with this phone number was not found.");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Error: Incorrect password.");
            }
           
            return user;
        }

        public async Task<LoginDto> LoginAsync(string username, string password )
        {
            var user = await ValidateUserAsync(username, password);

            if (user == null)
            {
                throw new ValidationException(new List<FluentValidation.Results.ValidationFailure>
                 {
                    new FluentValidation.Results.ValidationFailure("UserNameOrPassword", "Invalid UserName number or password")
                    {
                        ErrorCode = "1001"
                        
                    }
                });
            }





            await _userManager.UpdateAsync(user);
            var token = await GenerateJwtTokenAsync(user);

            return new LoginDto
            {
                Token = token,
            
            };
        }

       

      

       


    }
}
