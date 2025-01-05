namespace PrayerAppServices.PrayerGroups.Models {
    public class NewPrayerGroupResponse {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public string? Color { get; set; }
        public int? ImageFileId { get; set; }
    }
}
