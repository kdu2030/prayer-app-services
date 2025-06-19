
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    [ApiController]
    [Route("/api/v1/prayer-request")]
    public class PrayerRequestController(IPrayerRequestManager prayerRequestManager) : ControllerBase, IPrayerRequestController {
        private readonly IPrayerRequestManager _prayerRequestManager = prayerRequestManager;

        [HttpPost("/api/v1/prayergroup/{prayerGroupId}/prayer-request")]
        [Authorize]
        public async Task<ActionResult> CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest, CancellationToken token) {
            await _prayerRequestManager.CreatePrayerRequestAsync(prayerGroupId, createRequest, token);
            return Created();
        }

        [HttpPost("filter")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PrayerRequestModel>>> GetPrayerRequestsAsync(PrayerRequestFilterRequest request, CancellationToken token) {
            IEnumerable<PrayerRequestModel> prayerRequests = await _prayerRequestManager.GetPrayerRequestsAsync(request, token);
            return Ok(prayerRequests);
        }
    }
}
