using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupRepository {
        Task<CreatePrayerGroupResponse> CreatePrayerGroupAsync(string adminUsername, NewPrayerGroup newPrayerGroup);
        Task<PrayerGroup?> GetPrayerGroupByIdAsync(int id);
        Task<IEnumerable<PrayerGroupUserEntity>> GetPrayerGroupAdminsAsync(int prayerGroupId);
        Task<IEnumerable<PrayerGroupUserEntity>> GetPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupRole> prayerGroupRoles);
        Task<PrayerGroupAppUser?> GetPrayerGroupAppUserAsync(int prayerGroupId, string username);
        PrayerGroup? GetPrayerGroupByName(string groupName);
        IEnumerable<PrayerGroupSearchResult> SearchPrayerGroupsByName(string nameQuery, int maxNumResults);
    }
}
