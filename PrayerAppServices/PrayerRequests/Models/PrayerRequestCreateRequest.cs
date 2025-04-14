namespace PrayerAppServices.PrayerRequests.Models {
    public class PrayerRequestCreateRequest {
        public required int UserId { get; set; }
        public required string RequestTitle { get; set; }
        public required string RequestDescription { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
