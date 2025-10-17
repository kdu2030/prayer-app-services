using PrayerAppServices.PrayerGroups.Constants;

namespace PrayerAppServices.PrayerGroups.Models
{
    public class PrayerGroupRequest
    {
        public required string GroupName { get; set; }
        public required string Description { get; set; }
        public string? Rules { get; set; }
        public VisibilityLevel? VisibilityLevel { get; set; }
        public int? AvatarFileId { get; set; }
        public int? BannerFileId { get; set; }
    }
}
