namespace PrayerAppServices.PrayerGroups.Entities
{
    public class PrayerGroupDetailsEntity
    {
        public required int PrayerGroupId { get; set; }
        public required string GroupName { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public int? VisibilityLevel { get; set; }
        public int? AvatarFileId { get; set; }
        public string? GroupAvatarFileName { get; set; }
        public string? GroupAvatarFileUrl { get; set; }
        public int? BannerFileId { get; set; }
        public string? GroupBannerFileName { get; set; }
        public string? GroupBannerFileUrl { get; set; }
        public int? AdminUserId { get; set; }
        public string? AdminFullName { get; set; }
        public int? AdminImageFileId { get; set; }
        public string? AdminImageFileName { get; set; }
        public string? AdminImageFileUrl { get; set; }
    }
}
