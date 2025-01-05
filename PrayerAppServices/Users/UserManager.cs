using Isopoh.Cryptography.Argon2;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PrayerAppServices.Users.Entities;
using PrayerAppServices.Users.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrayerAppServices.Users {
    public class UserManager(UserManager<AppUser> userManager, IConfiguration configuration, JwtSecurityTokenHandler jwtSecurityTokenHandler) : IUserManager {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = jwtSecurityTokenHandler;

        private const int AccessTokenValidityMs = 60 * 60 * 1000;
        private const int RefreshTokenValidityMs = 15 * 24 * 60 * 60 * 1000;
        private readonly string BearerPrefix = "Bearer ";

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
            AppUser user = _userManager.Users.FirstOrDefault((user) => user.Id == userId) ?? throw new ArgumentException("User ID does not exist.");
            return CreateUserSummary(user);
        }

        public UserTokenPair GetUserTokenPair(string authHeader) {
            string username = ExtractUsernameFromToken(authHeader);
            return new UserTokenPair {
                AccessToken = GenerateToken(username, AccessTokenValidityMs),
                RefreshToken = GenerateToken(username, RefreshTokenValidityMs)
            };
        }

        public string ExtractUsernameFromToken(string authHeader) {
            string refreshToken = authHeader.Replace(BearerPrefix, string.Empty);
            JwtSecurityToken token = (JwtSecurityToken)_jwtSecurityTokenHandler.ReadToken(refreshToken);
            string username = token.Claims
                .Where((claim) => claim.Type == ClaimTypes.Name)
                .Select((claim) => claim.Value)
                .First();
            return username;
        }


        private UserSummary CreateUserSummary(AppUser user) {
            string username = user.UserName ?? "";

            return new UserSummary {
                Id = user.Id,
                Username = username,
                Email = user.Email ?? "",
                FullName = user.FullName ?? "",
                Tokens = new UserTokenPair {
                    AccessToken = GenerateToken(username, AccessTokenValidityMs),
                    RefreshToken = GenerateToken(username, RefreshTokenValidityMs)
                }
            };
        }

        private string GenerateToken(string username, int validityLengthMs) {
            string? jwtKey = _configuration["Jwt:Key"];
            string? issuer = _configuration["Jwt:Issuer"];
            string? audience = _configuration["Jwt:Audience"];

            if (jwtKey == null || issuer == null || audience == null) {
                throw new InvalidOperationException("Jwt:Key, Jwt:Issuer, and Jwt:Audience must be set in configuration.");
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            Claim[] claims = [
                new Claim(ClaimTypes.Name, username ?? "")
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
