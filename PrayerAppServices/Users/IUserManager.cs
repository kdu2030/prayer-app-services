using PrayerAppServices.Users.Models;

namespace PrayerAppServices.Users {
    public interface IUserManager {
        Task<UserSummary> CreateUserAsync(CreateUserRequest request);

        Task<UserSummary> GetUserSummaryFromCredentialsAsync(UserCredentials credentials);

        UserSummary GetUserSummaryFromUserId(int userId);

        UserTokenPair GetUserTokenPair(string authHeader);

        string ExtractUsernameFromToken(string authHeader);
    }
}
