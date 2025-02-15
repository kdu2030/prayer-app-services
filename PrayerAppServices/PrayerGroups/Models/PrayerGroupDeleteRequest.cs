namespace PrayerAppServices.PrayerGroups.Models {
    public class PrayerGroupDeleteRequest {
        public required IEnumerable<int> UserIds { get; set; }
    }
}
