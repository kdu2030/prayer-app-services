namespace PrayerAppServices.PrayerGroups.Models {
    public class NewPrayerGroupRequest {
        public required string GroupName { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public string? Color { get; set; }
        public int? ImageFileId { get; set; }
    }
}
