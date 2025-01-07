using PrayerAppServices.Files.Constants;
using PrayerAppServices.PrayerGroups.Constants;

namespace PrayerAppServices.PrayerGroups.Entities {
    public class PrayerGroupAdminUser {
        public int? Id { get; set; }
        public string? FullName { get; set; }
        public PrayerGroupRole? GroupRole { get; set; }
        public int? ImageFileId { get; set; }
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public FileType? FileType { get; set; }
    }
}
