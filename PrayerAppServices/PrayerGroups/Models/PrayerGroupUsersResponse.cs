using PrayerAppServices.Users.Models;

namespace PrayerAppServices.PrayerGroups.Models {
    public class PrayerGroupUsersResponse {
        public IEnumerable<UserSummary>? Users { get; set; }
    }
}
