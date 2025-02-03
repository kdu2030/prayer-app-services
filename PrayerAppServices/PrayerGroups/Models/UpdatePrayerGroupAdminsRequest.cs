namespace PrayerAppServices.PrayerGroups.Models {
    public class UpdatePrayerGroupAdminsRequest {
        public required IEnumerable<int> UserIds { get; set; }
    }
}
