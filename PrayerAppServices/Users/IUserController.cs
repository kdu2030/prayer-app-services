using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.Users.Models;

namespace PrayerAppServices.Users {

    public interface IUserController {
        Task<IActionResult> CreateUser(CreateUserRequest request);

        Task<IActionResult> GetUserSummaryFromCredentials(UserCredentials credentials);

        IActionResult GetUserSummaryFromUserId(int userId);

        IActionResult GetUserTokenPair(string authHeader);
    }
}
