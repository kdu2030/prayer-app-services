using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PrayerAppServices.User.Entities;
using PrayerAppServices.User.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PrayerAppServices.User {
    public class UserManager(UserManager<AppUser> userManager, IConfiguration configuration, JwtSecurityTokenHandler jwtSecurityTokenHandler) : IUserManager {
        private UserManager<AppUser> _userManager = userManager;
        private IConfiguration _configuration = configuration;
        private JwtSecurityTokenHandler _jwtSecurityTokenHandler = jwtSecurityTokenHandler;

        private const int AccessTokenValidityMs = 60 * 60 * 1000;
        private const int RefreshTokenValidityMs = 15 * 24 * 60 * 60 * 1000;

        public async Task<UserSummary> CreateUser(CreateUserRequest request) {
            Task<AppUser?> userByEmailResult = _userManager.FindByEmailAsync(request.Email);
            Task<AppUser?> userByUsernameResult = _userManager.FindByNameAsync(request.Username);

            AppUser?[] userResults = await Task.WhenAll(userByEmailResult, userByUsernameResult);

            if (userResults[0] is not null) {
                throw new ArgumentException("Email must be unique to each user.");
            }

            if (userResults[1] is not null) {
                throw new ArgumentException("Username must be unique to each user.");
            }

            string passwordHash = HashPassword(request.Password);
            AppUser newUser = new AppUser(request.Username, request.FullName, request.Email, passwordHash);
            await _userManager.CreateAsync(newUser);

            throw new NotImplementedException();
        }

        private string HashPassword(string rawPassword) {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(rawPassword);
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(passwordBytes);
            return Convert.ToHexString(hashBytes);
        }

        private string GenerateToken(AppUser user) {
            string? jwtKey = _configuration["Jwt:Key"];
            string? issuer = _configuration["Jwt:Issuer"];
            string? audience = _configuration["Jwt:Audience"];

            if (jwtKey == null || issuer == null || audience == null) {
                throw new InvalidOperationException("Jwt:Key, Jwt:Issuer, and Jwt:Audience must be set in configuration.");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            Claim[] claims = [
                new Claim(ClaimTypes.Name, user.UserName ?? "")
            ];

            throw new NotImplementedException();
        }

    }
}
