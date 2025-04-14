
using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    [ApiController]
    [Route("/api/v1")]
    public class PrayerRequestsController(IPrayerRequestManager prayerRequestManager) : ControllerBase, IPrayerRequestsController {
        private readonly IPrayerRequestManager _prayerRequestManager = prayerRequestManager;

        [HttpPost("prayergroup/{prayerGroupId}/prayer-request")]
        public async Task<ActionResult> CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest) {
            await _prayerRequestManager.CreatePrayerRequestAsync(prayerGroupId, createRequest);
            return Ok();
        }
    }
}
