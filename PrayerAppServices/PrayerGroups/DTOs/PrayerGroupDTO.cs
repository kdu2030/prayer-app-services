using PrayerAppServices.PrayerGroups.Constants;

namespace PrayerAppServices.PrayerGroups.DTOs
{
    public class PrayerGroupDTO
    {
        public required int CreatorUserId { get; set; }
        public required string NewGroupName { get; set; }
        public string? GroupDescription { get; set; }
        public string? GroupRules { get; set; }
        public VisibilityLevel? GroupVisibility { get; set; }
        public int? GroupAvatarFileId { get; set; }
        public int? GroupBannerFileId { get; set; }
    }
}
