using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    [ApiController]
    [Route("/api/v1/prayergroup")]
    public class PrayerGroupController(IPrayerGroupManager prayerGroupManager) : ControllerBase, IPrayerGroupController {
        private readonly IPrayerGroupManager _prayerGroupManager = prayerGroupManager;

        [HttpPost]
        [Authorize]
        public ActionResult<PrayerGroupDetails> CreatePrayerGroup([FromHeader(Name = "Authorization")] string authToken, NewPrayerGroupRequest newPrayerGroupRequest) {
            return Ok(_prayerGroupManager.CreatePrayerGroup(authToken, newPrayerGroupRequest));
        }
    }
}
