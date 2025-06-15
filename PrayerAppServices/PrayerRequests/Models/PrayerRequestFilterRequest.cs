namespace PrayerAppServices.PrayerRequests.Models {
    public class PrayerRequestFilterRequest {
        public required int UserId { get; set; }
        public required PrayerRequestFilterCriteria FilterCriteria { get; set; }
    }
}
