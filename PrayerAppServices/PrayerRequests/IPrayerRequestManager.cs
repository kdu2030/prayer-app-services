using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    public interface IPrayerRequestManager {
        Task CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest, CancellationToken token);
        Task<PrayerRequestGetResponse> GetPrayerRequestsAsync(PrayerRequestFilterRequest request, CancellationToken token);
        Task AddPrayerRequestLikeAsync(int userId, int prayerRequestId, CancellationToken token);
        Task RemovePrayerRequestLikeAsync(int userId, int prayerRequestId, CancellationToken token);
    }
}
