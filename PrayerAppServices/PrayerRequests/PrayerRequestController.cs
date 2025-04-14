
using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    [ApiController]
    [Route("/api/v1")]
    public class PrayerRequestController(IPrayerRequestManager prayerRequestManager) : ControllerBase, IPrayerRequestController {
        private readonly IPrayerRequestManager _prayerRequestManager = prayerRequestManager;

        [HttpPost("prayergroup/{prayerGroupId}/prayer-request")]
        public async Task<ActionResult> CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest, CancellationToken token) {
            await _prayerRequestManager.CreatePrayerRequestAsync(prayerGroupId, createRequest, token);
            return Created();
        }
    }
}
