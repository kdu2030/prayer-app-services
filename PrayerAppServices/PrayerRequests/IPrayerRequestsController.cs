using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    public interface IPrayerRequestsController {
        Task<ActionResult> CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest);
    }
}
