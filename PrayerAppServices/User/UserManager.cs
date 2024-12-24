using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PrayerAppServices.User.Entities;
using PrayerAppServices.User.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrayerAppServices.User {
    public class UserManager(UserManager<AppUser> userManager, IConfiguration configuration, JwtSecurityTokenHandler jwtSecurityTokenHandler) : IUserManager {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = jwtSecurityTokenHandler;

        private const int AccessTokenValidityMs = 60 * 60 * 1000;
        private const int RefreshTokenValidityMs = 15 * 24 * 60 * 60 * 1000;

        public async Task<UserSummary> CreateUserAsync(CreateUserRequest request) {
            Task<AppUser?> userByEmailResult = _userManager.FindByEmailAsync(request.Email);
            Task<AppUser?> userByUsernameResult = _userManager.FindByNameAsync(request.Username);

            AppUser?[] userResults = await Task.WhenAll(userByEmailResult, userByUsernameResult);

            if (userResults[0] is not null) {
                throw new ArgumentException("Email must be unique to each user.");
            }

            if (userResults[1] is not null) {
                throw new ArgumentException("Username must be unique to each user.");
            }

            string passwordHash = Argon2.Hash(request.Password);
            AppUser newUser = new AppUser(request.Username, request.FullName, request.Email, passwordHash);
            await _userManager.CreateAsync(newUser);

            return CreateUserSummary(newUser);
        }

        public async Task<UserSummary> GetUserSummaryFromCredentialsAsync(UserCredentials credentials) {
            AppUser? user = await _userManager.FindByEmailAsync(credentials.Email) ?? throw new ArgumentException("A User with this email does not exist.");
            bool passwordValid = Argon2.Verify(user.PasswordHash, credentials.Password);
            if (!passwordValid) {
                throw new UnauthorizedAccessException("Password is incorrect.");
            }

            return CreateUserSummary(user);
        }

        public UserSummary GetUserSummaryFromUserId(int userId) {
            AppUser user = userManager.Users.FirstOrDefault((user) => user.Id == userId) ?? throw new ArgumentException("User ID does not exist.");
            return CreateUserSummary(user);
        }


        private UserSummary CreateUserSummary(AppUser user) {
            return new UserSummary {
                Id = user.Id,
                Username = user.UserName ?? "",
                Email = user.Email ?? "",
                FullName = user.FullName ?? "",
                Tokens = new UserTokenPair {
                    AccessToken = GenerateToken(user, AccessTokenValidityMs),
                    RefreshToken = GenerateToken(user, RefreshTokenValidityMs)
                }
            };
        }

        private string GenerateToken(AppUser user, int validityLengthMs) {
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

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMilliseconds(validityLengthMs),
                signingCredentials: credentials
            );

            return _jwtSecurityTokenHandler.WriteToken(token);
        }

    }
}
