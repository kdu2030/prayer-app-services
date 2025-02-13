using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    [ApiController]
    [Route("/api/v1/prayergroup")]
    public class PrayerGroupController(IPrayerGroupManager prayerGroupManager) : ControllerBase, IPrayerGroupController {
        private readonly IPrayerGroupManager _prayerGroupManager = prayerGroupManager;

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PrayerGroupDetails>> CreatePrayerGroupAsync([FromHeader(Name = "Authorization")] string authHeader, PrayerGroupRequest newPrayerGroupRequest) {
            PrayerGroupDetails details = await _prayerGroupManager.CreatePrayerGroupAsync(authHeader, newPrayerGroupRequest);
            return Ok(details);
        }

        [HttpGet("{prayerGroupId}")]
        [Authorize]
        public async Task<ActionResult<PrayerGroupDetails>> GetPrayerGroupDetailsAsync([FromHeader(Name = "Authorization")] string authHeader, int prayerGroupId) {
            PrayerGroupDetails prayerGroupDetails = await _prayerGroupManager.GetPrayerGroupDetailsAsync(authHeader, prayerGroupId);
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

        [HttpPut("{prayerGroupId}")]
        [Authorize]
        public async Task<ActionResult<PrayerGroupDetails>> UpdatePrayerGroupAsync(int prayerGroupId, PrayerGroupRequest prayerGroupRequest) {
            PrayerGroupDetails prayerGroup = await _prayerGroupManager.UpdatePrayerGroupAsync(prayerGroupId, prayerGroupRequest);
            return Ok(prayerGroup);
        }

        [HttpGet("{prayerGroupId}/users")]
        [Authorize]
        public async Task<ActionResult<PrayerGroupUsersResponse>> GetPrayerGroupUsersAsync(int prayerGroupId, [FromQuery(Name = "role")] IEnumerable<PrayerGroupRole>? roles) {
            PrayerGroupUsersResponse prayerGroupUsersResponse = await _prayerGroupManager.GetPrayerGroupUsersAsync(prayerGroupId, roles);
            return Ok(prayerGroupUsersResponse);
        }

        [HttpPut("{prayerGroupId}/admins")]
        [Authorize]
        public async Task<ActionResult> UpdatePrayerGroupAdminsAsync(int prayerGroupId, UpdatePrayerGroupAdminsRequest updateAdminsRequest) {
            await _prayerGroupManager.UpdatePrayerGroupAdminsAsync(prayerGroupId, updateAdminsRequest);
            return Ok();
        }

        [HttpPost("{prayerGroupId}/users")]
        [Authorize]
        public async Task<ActionResult> AddPrayerGroupUsersAsync(int prayerGroupId, AddPrayerGroupUserRequest request) {
            await _prayerGroupManager.AddPrayerGroupUsersAsync(prayerGroupId, request);
            return Ok();
        }

        [HttpDelete("{prayerGroupId}/users")]
        [Authorize]
        public async Task<ActionResult> DeletePrayerGroupUsersAsync([FromHeader(Name = "Authorization")] string authHeader, int prayerGroupId, PrayerGroupDeleteRequest request) {
            await _prayerGroupManager.DeletePrayerGroupUsersAsync(authHeader, prayerGroupId, request);
            return Ok();
        }

    }
}
