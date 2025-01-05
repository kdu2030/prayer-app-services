namespace PrayerAppServices.PrayerGroups.Entities {
    public class CreatePrayerGroupResponse {
        public required int AdminUserId { get; set; }
        public string? AdminUserName { get; set; }
        public int? AdminImageFileId { get; set; }
    }
}
