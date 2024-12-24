using PrayerAppServices.User.Models;

namespace PrayerAppServices.User {
    public interface IUserManager {
        Task<UserSummary> CreateUserAsync(CreateUserRequest request);

        Task<UserSummary> GetUserSummaryFromCredentialsAsync(UserCredentials credentials);
    }
}
