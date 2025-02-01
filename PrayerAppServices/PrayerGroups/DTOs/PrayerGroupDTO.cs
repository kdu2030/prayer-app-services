namespace PrayerAppServices.PrayerGroups.DTOs {
    public class PrayerGroupDTO {
        public required string GroupName { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public int? Color { get; set; }
        public int? ImageFileId { get; set; }
    }
}
