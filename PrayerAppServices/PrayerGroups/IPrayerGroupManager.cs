using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupManager {
        PrayerGroupDetails CreatePrayerGroup(string authToken, NewPrayerGroupRequest newPrayerGroupRequest);
        Task<PrayerGroupDetails> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId);
    }
}
