namespace PrayerAppServices.PrayerGroups.Models {
    public class PrayerGroupRequest {
        public required string GroupName { get; set; }
        public required string Description { get; set; }
        public string? Rules { get; set; }
        public string? Color { get; set; }
        public int? ImageFileId { get; set; }
    }
}
