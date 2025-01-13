using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.PrayerGroups.Entities {
    public class PrayerGroupUser {
        public int? Id { get; set; }
        public required PrayerGroup PrayerGroup { get; set; }
        public required AppUser AppUser { get; set; }
        public required PrayerGroupRole Role { get; set; }

    }
}
