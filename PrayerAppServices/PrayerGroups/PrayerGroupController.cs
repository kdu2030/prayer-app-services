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
        public async Task<ActionResult<PrayerGroupDetails>> CreatePrayerGroupAsync([FromHeader(Name = "Authorization")] string authHeader, NewPrayerGroupRequest newPrayerGroupRequest) {
            PrayerGroupDetails details = await _prayerGroupManager.CreatePrayerGroupAsync(authHeader, newPrayerGroupRequest);
            return Ok(details);
        }

        [HttpGet("{prayerGroupId}")]
        [Authorize]
        public ActionResult<PrayerGroupDetails> GetPrayerGroupDetails([FromHeader(Name = "Authorization")] string authHeader, int prayerGroupId) {
            PrayerGroupDetails prayerGroupDetails = _prayerGroupManager.GetPrayerGroupDetails(authHeader, prayerGroupId);
            return Ok(prayerGroupDetails);
        }

        [HttpGet("validate-name")]
        [Authorize]
        public ActionResult<GroupNameValidationResponse> ValidateGroupName([FromQuery(Name = "name")] string prayerGroupName) {
            GroupNameValidationResponse validationResponse = _prayerGroupManager.ValidateGroupName(prayerGroupName);
            return Ok(validationResponse);
        }

        [HttpGet("search")]
        [Authorize]
        public ActionResult<IEnumerable<PrayerGroupDetails>> SearchByGroupName([FromQuery(Name = "name")] string nameQuery, [FromQuery(Name = "maxResults")] int maxResults) {
            IEnumerable<PrayerGroupDetails> prayerGroups = _prayerGroupManager.SearchPrayerGroupsByName(nameQuery, maxResults);
            return Ok(prayerGroups);

        }
    }
}
