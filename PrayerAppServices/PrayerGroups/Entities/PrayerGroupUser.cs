using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.PrayerGroups.Entities
{
    public class PrayerGroupUser
    {
        public int? PrayerGroupUserId { get; set; }
        public required PrayerGroup PrayerGroup { get; set; }
        public required AppUser AppUser { get; set; }
        public required PrayerGroupRole PrayerGroupRole { get; set; }

    }
}
