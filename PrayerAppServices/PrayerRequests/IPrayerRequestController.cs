using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    public interface IPrayerRequestController {
        Task<ActionResult> CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest);
    }
}
