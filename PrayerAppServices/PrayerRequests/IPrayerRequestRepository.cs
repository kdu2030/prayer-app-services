using PrayerAppServices.PrayerRequests.Entities;

namespace PrayerAppServices.PrayerRequests {
    public interface IPrayerRequestRepository {
        Task CreatePrayerRequestAsync(PrayerRequest prayerRequest);
    }
}
