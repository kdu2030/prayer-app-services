namespace PrayerAppServices.PrayerRequests.Models {
    public class PrayerRequestGetResponse {
        public required IEnumerable<PrayerRequestModel> PrayerRequests { get; set; }
        public required int TotalCount { get; set; }

    }
}
