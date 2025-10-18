using PrayerAppServices.Files.Entities;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.Users.Models;

namespace PrayerAppServices.PrayerGroups.Models
{
    public class PrayerGroupModel
    {
        public int? PrayerGroupId { get; set; }
        public string? GroupName { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public VisibilityLevel? VisibilityLevel { get; set; }
        public MediaFileBase? AvatarFile { get; set; }
        public MediaFileBase? BannerFile { get; set; }
        public IEnumerable<UserSummary>? Admins { get; set; }
        public JoinStatus? JoinStatus { get; set; }
        public PrayerGroupRole? PrayerGroupRole { get; set; }

    }
}
