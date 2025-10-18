using PrayerAppServices.Files.Constants;
using PrayerAppServices.PrayerGroups.Constants;

namespace PrayerAppServices.PrayerGroups.Entities
{
    public class PrayerGroupUserEntity
    {
        public int? UserId { get; set; }
        public string? FullName { get; set; }
        public string? Username { get; set; }
        public PrayerGroupRole? PrayerGroupRole { get; set; }
        public int? ImageFileId { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public FileType? FileType { get; set; }
    }
}
