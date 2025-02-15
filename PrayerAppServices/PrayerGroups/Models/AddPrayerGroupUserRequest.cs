using PrayerAppServices.PrayerGroups.Entities;

namespace PrayerAppServices.PrayerGroups.Models {
    public class AddPrayerGroupUserRequest {
        public required IEnumerable<PrayerGroupAppUser> Users { get; set; }
    }
}
