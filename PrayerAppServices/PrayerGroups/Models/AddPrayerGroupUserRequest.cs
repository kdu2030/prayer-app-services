using PrayerAppServices.PrayerGroups.Entities;

namespace PrayerAppServices.PrayerGroups.Models {
    public class AddPrayerGroupUserRequest {
        public IEnumerable<PrayerGroupAppUser> Users { get; set; }
    }
}
