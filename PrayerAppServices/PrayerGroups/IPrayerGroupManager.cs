using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupManager {
        PrayerGroupDetails CreatePrayerGroup(string authToken, NewPrayerGroupRequest newPrayerGroupRequest);
    }
}
