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
        public ActionResult<PrayerGroupDetails> CreatePrayerGroup([FromHeader(Name = "Authorization")] string authHeader, NewPrayerGroupRequest newPrayerGroupRequest) {
            return Ok(_prayerGroupManager.CreatePrayerGroup(authHeader, newPrayerGroupRequest));
        }

        [HttpGet("{prayerGroupId}")]
        [Authorize]
        public ActionResult<PrayerGroupDetails> GetPrayerGroupDetails([FromHeader(Name = "Authorization")] string authHeader, int prayerGroupId) {
            PrayerGroupDetails prayerGroupDetails = _prayerGroupManager.GetPrayerGroupDetails(authHeader, prayerGroupId);
            return Ok(prayerGroupDetails);
        }
    }
}
