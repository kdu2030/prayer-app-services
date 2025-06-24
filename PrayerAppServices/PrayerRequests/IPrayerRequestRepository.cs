using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    public interface IPrayerRequestRepository {
        Task CreatePrayerRequestAsync(PrayerRequest prayerRequest, CancellationToken token = default);
        Task<IEnumerable<PrayerRequest>> GetPrayerRequestsAsync(PrayerRequestFilterCriteria filterCriteria, CancellationToken token);
        Task<UserPrayerRequestData> GetPrayerRequestUserDataAsync(int userId, CancellationToken token);
        Task AddPrayerRequestLikeAsync(int prayerRequestId, int userId, CancellationToken token);
        Task RemovePrayerRequestLikeAsync(int prayerRequestId, int userId, CancellationToken token);
    }
}
