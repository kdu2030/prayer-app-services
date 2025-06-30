using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    public interface IPrayerRequestManager {
        Task CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest, CancellationToken token);
        Task<PrayerRequestGetResponse> GetPrayerRequestsAsync(PrayerRequestFilterRequest request, CancellationToken token);
    }
}
