using PrayerAppServices.Files.Constants;

namespace PrayerAppServices.PrayerGroups.Entities {
    public class PrayerGroupSummaryEntity {
        public int Id { get; set; }
        public required string GroupName { get; set; }
        public int? ImageFileId { get; set; }
        public string? FileName { get; set; }
        public string? Url { get; set; }
        public FileType? FileType { get; set; }
    }
}
