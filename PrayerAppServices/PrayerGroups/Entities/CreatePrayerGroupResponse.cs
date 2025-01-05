namespace PrayerAppServices.PrayerGroups.Entities {
    public class CreatePrayerGroupResponse {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public int? Color { get; set; }
        public int? ImageFileId { get; set; }
        public string? GroupImageFileName { get; set; }
        public string? GroupImageFileUrl { get; set; }
        public int? AdminUserId { get; set; }
        public string? AdminFullName { get; set; }
        public int? AdminImageFileId { get; set; }
        public string? AdminImageFileName { get; set; }
        public string? AdminImageFileUrl { get; set; }
    }
}
