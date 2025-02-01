using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupManager {
        Task<PrayerGroupDetails> CreatePrayerGroupAsync(string authToken, NewPrayerGroupRequest newPrayerGroupRequest);
        PrayerGroupDetails GetPrayerGroupDetails(string authHeader, int prayerGroupId);
        GroupNameValidationResponse ValidateGroupName(string groupName);
        IEnumerable<PrayerGroupDetails> SearchPrayerGroupsByName(string nameQuery, int maxNumResults);
    }
}
