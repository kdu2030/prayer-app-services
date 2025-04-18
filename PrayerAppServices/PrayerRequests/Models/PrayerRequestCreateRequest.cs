using System.ComponentModel.DataAnnotations;

namespace PrayerAppServices.PrayerRequests.Models {
    public class PrayerRequestCreateRequest {
        public required int UserId { get; set; }

        [MaxLength(255)]
        public required string RequestTitle { get; set; }

        public required string RequestDescription { get; set; }

        public DateTime? ExpirationDate { get; set; }
    }
}
