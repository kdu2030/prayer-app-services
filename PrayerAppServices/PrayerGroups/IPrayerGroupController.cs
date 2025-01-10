using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupController {
        ActionResult<PrayerGroupDetails> CreatePrayerGroup(string authHeader, NewPrayerGroupRequest newPrayerGroupRequest);
        ActionResult<PrayerGroupDetails> GetPrayerGroupDetails(string authHeader, int prayerGroupId);
    }
}
