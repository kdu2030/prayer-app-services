using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.PrayerRequests.Entities {
    public class PrayerRequestLike {
        public int? Id { get; set; }
        public required PrayerRequest PrayerRequest { get; set; }
        public required AppUser AppUser { get; set; }
    }
}
