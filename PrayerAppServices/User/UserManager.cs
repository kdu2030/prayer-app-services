using Microsoft.AspNetCore.Identity;
using PrayerAppServices.User.Entities;
using PrayerAppServices.User.Models;
using System.Security.Cryptography;
using System.Text;

namespace PrayerAppServices.User {
    public class UserManager(UserManager<AppUser> userManager) {
        private UserManager<AppUser> _userManager = userManager;

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
    }
}
