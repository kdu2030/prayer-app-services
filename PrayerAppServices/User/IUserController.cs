using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.User.Models;

namespace PrayerAppServices.User {

    public interface IUserController {
        public Task<IActionResult> CreateUser(CreateUserRequest request);

        public Task<IActionResult> GetUserSummaryFromCredentials(UserCredentials credentials);
    }
}
