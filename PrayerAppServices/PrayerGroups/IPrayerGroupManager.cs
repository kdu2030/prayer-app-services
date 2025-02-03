using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupManager {
        Task<PrayerGroupDetails> CreatePrayerGroupAsync(string authToken, PrayerGroupRequest newPrayerGroupRequest);
        Task<PrayerGroupDetails> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId);
        GroupNameValidationResponse ValidateGroupName(string groupName);
        IEnumerable<PrayerGroupDetails> SearchPrayerGroupsByName(string nameQuery, int maxNumResults);
        Task<PrayerGroupDetails> UpdatePrayerGroupAsync(int prayerGroupId, PrayerGroupRequest prayerGroupRequest);
        Task<PrayerGroupUsersResponse> GetPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupRole>? prayerGroupRoles);
        Task UpdatePrayerGroupAdminsAsync(int prayerGroupId, UpdatePrayerGroupAdminsRequest updateAdminsRequest);
    }
}
