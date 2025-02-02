using PrayerAppServices.PrayerGroups.DTOs;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupManager {
        Task<PrayerGroupDetails> CreatePrayerGroupAsync(string authToken, PrayerGroupRequest newPrayerGroupRequest);
        Task<PrayerGroupDetails> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId);
        GroupNameValidationResponse ValidateGroupName(string groupName);
        IEnumerable<PrayerGroupDetails> SearchPrayerGroupsByName(string nameQuery, int maxNumResults);
        Task<PrayerGroupDetails> UpdatePrayerGroupAsync(int prayerGroupId, PrayerGroupRequest prayerGroupRequest);
    }
}
