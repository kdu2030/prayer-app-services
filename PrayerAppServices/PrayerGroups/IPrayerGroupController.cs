using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupController {
        ActionResult<PrayerGroupDetails> CreatePrayerGroup(string authToken, NewPrayerGroupRequest newPrayerGroupRequest);
    }
}
