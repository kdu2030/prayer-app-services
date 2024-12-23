using PrayerAppServices.User.Models;

namespace PrayerAppServices.User {
    public interface IUserManager {
        Task<UserSummary> CreateUser(CreateUserRequest request);
    }
}
