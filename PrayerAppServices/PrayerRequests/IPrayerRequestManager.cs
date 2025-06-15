using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    public interface IPrayerRequestManager {
        Task CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest, CancellationToken token);
        Task<IEnumerable<PrayerRequestModel>> GetPrayerRequestsAsync(int userId, PrayerRequestFilterCriteria filterCriteria, CancellationToken token);
    }
}
