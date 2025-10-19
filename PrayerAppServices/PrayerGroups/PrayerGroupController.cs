using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups
{
    [ApiController]
    [Route("/api/v1/prayergroup")]
    public class PrayerGroupController(IPrayerGroupManager prayerGroupManager) : ControllerBase, IPrayerGroupController
    {
        private readonly IPrayerGroupManager _prayerGroupManager = prayerGroupManager;

        /// <summary>
        /// Creates a prayer group
        /// </summary>
        /// <param name="authHeader"></param>
        /// <param name="newPrayerGroupRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PrayerGroupModel>> CreatePrayerGroupAsync([FromHeader(Name = "Authorization")] string authHeader, PrayerGroupRequest newPrayerGroupRequest)
        {
            PrayerGroupModel details = await _prayerGroupManager.CreatePrayerGroupAsync(authHeader, newPrayerGroupRequest);
            return Ok(details);
        }

        /// <summary>
        /// Gets a prayer group
        /// </summary>
        /// <param name="authHeader"></param>
        /// <param name="prayerGroupId"></param>
        /// <returns></returns>
        [HttpGet("{prayerGroupId}")]
        [Authorize]
        public async Task<ActionResult<PrayerGroupModel>> GetPrayerGroupDetailsAsync([FromHeader(Name = "Authorization")] string authHeader, int prayerGroupId)
        {
            PrayerGroupModel prayerGroupDetails = await _prayerGroupManager.GetPrayerGroupDetailsAsync(authHeader, prayerGroupId);
            return Ok(prayerGroupDetails);
        }

        /// <summary>
        /// Validates prayer group name for uniqueness
        /// </summary>
        /// <param name="prayerGroupName"></param>
        /// <returns></returns>
        [HttpGet("validate-name")]
        [Authorize]
        public async Task<ActionResult<GroupNameValidationResponse>> ValidateGroupNameAsync([FromQuery(Name = "name")] string prayerGroupName)
        {
            GroupNameValidationResponse validationResponse = await _prayerGroupManager.ValidateGroupNameAsync(prayerGroupName);
            return Ok(validationResponse);
        }

        /// <summary>
        /// Search for prayer groups by name
        /// </summary>
        /// <param name="prayerGroupSearchRequest"></param>
        /// <returns></returns>
        [HttpPost("search")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PrayerGroupModel>>> SearchPrayerGroupsAsync([FromBody] PrayerGroupSearchRequest prayerGroupSearchRequest)
        {
            IEnumerable<PrayerGroupModel> prayerGroups = await _prayerGroupManager.SearchPrayerGroupsAsync(prayerGroupSearchRequest);
            return Ok(prayerGroups);
        }

        /// <summary>
        /// Updates prayer group
        /// </summary>
        /// <param name="authHeader"></param>
        /// <param name="prayerGroupId"></param>
        /// <param name="prayerGroupRequest"></param>
        /// <returns></returns>
        [HttpPut("{prayerGroupId}")]
        [Authorize]
        public async Task<ActionResult<PrayerGroupModel>> UpdatePrayerGroupAsync([FromHeader(Name = "Authorization")] string authHeader, int prayerGroupId, PrayerGroupRequest prayerGroupRequest)
        {
            PrayerGroupModel prayerGroup = await _prayerGroupManager.UpdatePrayerGroupAsync(authHeader, prayerGroupId, prayerGroupRequest);
            return Ok(prayerGroup);
        }

        /// <summary>
        /// Gets users that have joined a prayer group
        /// </summary>
        /// <param name="prayerGroupId"></param>
        /// <param name="getUsersRequest"></param>
        /// <returns></returns>
        [HttpPost("{prayerGroupId}/users")]
        [Authorize]
        public async Task<ActionResult<PrayerGroupUsersResponse>> GetPrayerGroupUsersAsync(int prayerGroupId, [FromBody] PrayerGroupUsersGetRequest getUsersRequest)
        {
            PrayerGroupUsersResponse prayerGroupUsersResponse = await _prayerGroupManager.GetPrayerGroupUsersAsync(prayerGroupId, getUsersRequest);
            return Ok(prayerGroupUsersResponse);
        }


        /// <summary>
        /// Add a prayer group user
        /// </summary>
        /// <param name="authHeader"></param>
        /// <param name="prayerGroupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost("{prayerGroupId}/user/{userId}")]
        [Authorize]
        public async Task<ActionResult> AddPrayerGroupUserAsync([FromHeader(Name = "Authorization")] string authHeader, int prayerGroupId, int userId)
        {
            await _prayerGroupManager.AddPrayerGroupUserAsync(authHeader, prayerGroupId, userId);
            return Ok();
        }

        [HttpPut("{prayerGroupId}/admins")]
        [Authorize]
        public async Task<ActionResult> UpdatePrayerGroupAdminsAsync([FromHeader(Name = "Authorization")] string authHeader, int prayerGroupId, UpdatePrayerGroupAdminsRequest updateAdminsRequest)
        {
            await _prayerGroupManager.UpdatePrayerGroupAdminsAsync(authHeader, prayerGroupId, updateAdminsRequest);
            return Ok();
        }

        //[HttpPost("{prayerGroupId}/users")]
        //[Authorize]
        //public async Task<ActionResult> AddPrayerGroupUsersAsync(int prayerGroupId, AddPrayerGroupUserRequest request)
        //{
        //    await _prayerGroupManager.AddPrayerGroupUsersAsync(prayerGroupId, request);
        //    return Ok();
        //}

        [HttpDelete("{prayerGroupId}/users")]
        [Authorize]
        public async Task<ActionResult> DeletePrayerGroupUsersAsync([FromHeader(Name = "Authorization")] string authHeader, int prayerGroupId, PrayerGroupDeleteRequest request)
        {
            await _prayerGroupManager.DeletePrayerGroupUsersAsync(authHeader, prayerGroupId, request);
            return Ok();
        }

    }
}
