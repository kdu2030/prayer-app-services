
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
        public async Task<ActionResult<PrayerRequestGetResponse>> GetPrayerRequestsAsync(PrayerRequestFilterRequest request, CancellationToken token) {
            PrayerRequestGetResponse prayerRequestsResponse = await _prayerRequestManager.GetPrayerRequestsAsync(request, token);
            return Ok(prayerRequestsResponse);
        }

        [HttpPost("{prayerRequestId}/like")]
        [Authorize]
        public async Task<ActionResult> AddPrayerRequestLikeAsync(int prayerRequestId, [FromQuery] int userId, CancellationToken token) {
            await _prayerRequestManager.AddPrayerRequestLikeAsync(userId, prayerRequestId, token);
            return Ok();
        }

        [HttpDelete("{prayerRequestId}/like")]
        [Authorize]
        public async Task<ActionResult> RemovePrayerRequestLikeAsync(int prayerRequestId, [FromQuery] int userId, CancellationToken token) {
            await _prayerRequestManager.RemovePrayerRequestLikeAsync(userId, prayerRequestId, token);
            return Ok();
        }
    }
}
