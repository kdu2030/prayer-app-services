﻿using PrayerAppServices.Users.Models;

namespace PrayerAppServices.Users {
    public interface IUserManager {
        Task<UserSummary> CreateUserAsync(CreateUserRequest request);

        Task<UserSummary> GetUserSummaryFromCredentialsAsync(UserCredentials credentials);

        Task<UserSummary> GetUserSummaryFromUserIdAsync(int userId);

        UserTokenPair GetUserTokenPair(string authHeader);

        string ExtractUsernameFromAuthHeader(string authHeader);
    }
}
