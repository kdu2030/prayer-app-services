using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.Users.Models;

namespace PrayerAppServices.PrayerGroups.Models {
    public class PrayerGroupUserSummary : UserSummary {
        public PrayerGroupRole Role { get; set; }
    }
}
