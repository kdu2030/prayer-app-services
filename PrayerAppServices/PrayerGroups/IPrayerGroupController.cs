using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupController {
        Task<ActionResult<PrayerGroupDetails>> CreatePrayerGroupAsync(string authHeader, PrayerGroupRequest newPrayerGroupRequest);
        Task<ActionResult<PrayerGroupDetails>> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId);
        ActionResult<GroupNameValidationResponse> ValidateGroupName(string prayerGroupName);
        ActionResult<IEnumerable<PrayerGroupDetails>> SearchByGroupName(string nameQuery, int maxResults);
        Task<ActionResult<PrayerGroupDetails>> UpdatePrayerGroupAsync(int prayerGroupId, PrayerGroupRequest prayerGroupRequest);
    }
}
