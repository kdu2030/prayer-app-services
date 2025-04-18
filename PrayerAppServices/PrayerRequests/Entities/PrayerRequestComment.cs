using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.PrayerRequests.Entities {
    public class PrayerRequestComment {
        public int? Id { get; set; }
        public required PrayerRequest PrayerRequest { get; set; }
        public required AppUser User { get; set; }
        public required string Comment { get; set; }
        public required DateTime CreatedDate { get; set; }
    }
}
