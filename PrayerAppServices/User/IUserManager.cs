using PrayerAppServices.User.Models;

namespace PrayerAppServices.User {
    public interface IUserManager {
        Task<UserSummary> CreateUserAsync(CreateUserRequest request);

        Task<UserSummary> GetUserSummaryFromCredentialsAsync(UserCredentials credentials);

        UserSummary GetUserSummaryFromUserId(int userId);

        UserTokenPair GetUserTokenPair(string authHeader);
    }
}
