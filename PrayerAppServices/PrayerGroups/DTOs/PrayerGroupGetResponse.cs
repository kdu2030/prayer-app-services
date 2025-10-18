namespace PrayerAppServices.PrayerGroups.DTOs
{
    public class PrayerGroupGetResponse
    {
        public int PrayerGroupId { get; set; }
        public string? GroupName { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public int VisibilityLevel { get; set; }

        public int? AvatarFileId { get; set; }
        public string? AvatarFileName { get; set; }
        public string? AvatarFileUrl { get; set; }
        public int? AvatarFileType { get; set; }

        public int? BannerFileId { get; set; }
        public string? BannerFileName { get; set; }
        public string? BannerFileUrl { get; set; }
        public int? BannerFileType { get; set; }

        public int? PrayerGroupRole { get; set; }
        public int? JoinRequestId { get; set; }
    }
}
