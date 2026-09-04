using ECommerce.Business.Interfaces;
using ECommerce.Data.Repositories.Interfaces;
using ECommerce.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<bool> RegisterAsync(
            string name,
            string email,
            string password)
        {
            var existingUser =
                await _userRepository.GetByEmailAsync(email);

            if (existingUser != null)
            {
                return false;
            }

            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(password),
                Role = "Customer"

            };

            await _userRepository.AddAsync(user);

            return true;
        }

        public async Task<string?> LoginAsync(
            string email,
            string password)
        {
            var user =
                await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            bool passwordValid =
                BCrypt.Net.BCrypt.Verify(
                    password,
                    user.PasswordHash);

            if (!passwordValid)
            {
                return null;
            }

            // Create claims
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.Name),

                new Claim(
                    ClaimTypes.Email,
                    user.Email),

                new Claim(
                    ClaimTypes.Role,
                    user.Role)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}