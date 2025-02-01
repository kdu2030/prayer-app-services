using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupManager {
        Task<PrayerGroupDetails> CreatePrayerGroupAsync(string authToken, NewPrayerGroupRequest newPrayerGroupRequest);
        Task<PrayerGroupDetails> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId);
        GroupNameValidationResponse ValidateGroupName(string groupName);
        IEnumerable<PrayerGroupDetails> SearchPrayerGroupsByName(string nameQuery, int maxNumResults);
    }
}
