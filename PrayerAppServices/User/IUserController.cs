using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.User.Models;

namespace PrayerAppServices.User {

    public interface IUserController {
        public Task<IActionResult> CreateUser(CreateUserRequest request);

        public Task<IActionResult> GetUserSummaryFromCredentials(UserCredentials credentials);

        public IActionResult GetUserSummaryFromUserId(int userId);

        public IActionResult GetUserTokenPair(string authHeader);
    }
}
