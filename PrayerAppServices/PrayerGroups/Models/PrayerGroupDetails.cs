using PrayerAppServices.Files.Entities;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.Users.Models;

namespace PrayerAppServices.PrayerGroups.Models {
    public class PrayerGroupDetails {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public string? Color { get; set; }
        public MediaFileBase? ImageFile { get; set; }
        public IEnumerable<UserSummary>? Admins { get; set; }
        public bool? IsUserJoined { get; set; }
        public PrayerGroupRole? UserRole { get; set; }

    }
}
