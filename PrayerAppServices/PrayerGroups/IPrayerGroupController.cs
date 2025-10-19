using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups
{
    public interface IPrayerGroupController
    {
        Task<ActionResult<PrayerGroupModel>> CreatePrayerGroupAsync(string authHeader, PrayerGroupRequest newPrayerGroupRequest);
        Task<ActionResult<PrayerGroupModel>> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId);
        Task<ActionResult<GroupNameValidationResponse>> ValidateGroupNameAsync(string prayerGroupName);
        Task<ActionResult<IEnumerable<PrayerGroupModel>>> SearchPrayerGroupsAsync([FromBody] PrayerGroupSearchRequest prayerGroupSearchRequest);
        Task<ActionResult<PrayerGroupModel>> UpdatePrayerGroupAsync([FromHeader(Name = "Authorization")] string authHeader, int prayerGroupId, PrayerGroupRequest prayerGroupRequest);
        Task<ActionResult<PrayerGroupUsersResponse>> GetPrayerGroupUsersAsync(int prayerGroupId, PrayerGroupUsersGetRequest getUsersRequest);
        Task<ActionResult> UpdatePrayerGroupAdminsAsync(string authHeader, int prayerGroupId, UpdatePrayerGroupAdminsRequest updateAdminsRequest);
        //Task<ActionResult> AddPrayerGroupUsersAsync(int prayerGroupId, AddPrayerGroupUserRequest request);
        Task<ActionResult> DeletePrayerGroupUsersAsync(string authHeader, int prayerGroupId, PrayerGroupDeleteRequest request);
    }
}
