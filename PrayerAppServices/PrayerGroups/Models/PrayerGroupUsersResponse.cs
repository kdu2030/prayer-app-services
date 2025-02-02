using PrayerAppServices.Users.Models;

namespace PrayerAppServices.PrayerGroups.Models {
    public class PrayerGroupUsersResponse {
        public IEnumerable<PrayerGroupUserSummary>? Users { get; set; }
    }
}
