using PrayerAppServices.PrayerRequests.Entities;

namespace PrayerAppServices.PrayerRequests.DTOs {
    public class PrayerRequestResponseDTO {
        public required IEnumerable<PrayerRequest> PrayerRequests { get; set; }
        public required int TotalCount { get; set; }
    }
}
