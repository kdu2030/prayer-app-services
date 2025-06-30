using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    public interface IPrayerRequestController {
        Task<ActionResult> CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest, CancellationToken token);
        Task<ActionResult<PrayerRequestGetResponse>> GetPrayerRequestsAsync(PrayerRequestFilterRequest request, CancellationToken token);
        Task<ActionResult> AddPrayerRequestLikeAsync(int prayerRequestId, [FromQuery] int userId, CancellationToken token);
        Task<ActionResult> RemovePrayerRequestLikeAsync(int prayerRequestId, [FromQuery] int userId, CancellationToken token);
    }
}
