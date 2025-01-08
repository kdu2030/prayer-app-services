using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupController {
        ActionResult<PrayerGroupDetails> CreatePrayerGroup(string authHeader, NewPrayerGroupRequest newPrayerGroupRequest);
        Task<ActionResult<PrayerGroupDetails>> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId);
    }
}
