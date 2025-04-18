using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.DTOs;
using PrayerAppServices.PrayerGroups.Entities;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupRepository {
        Task<PrayerGroupDetailsEntity> CreatePrayerGroupAsync(string adminUsername, PrayerGroupDTO newPrayerGroup);
        Task<PrayerGroup?> GetPrayerGroupByIdAsync(int id, bool includeImage = false);
        Task<IEnumerable<PrayerGroupUserEntity>> GetPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupRole> prayerGroupRoles);
        Task<PrayerGroupAppUser?> GetPrayerGroupAppUserByUsernameAsync(int prayerGroupId, string username);
        Task<PrayerGroup?> GetPrayerGroupByNameAsync(string groupName, bool enableTracking = true);
        IEnumerable<PrayerGroupSearchResult> SearchPrayerGroupsByName(string nameQuery, int maxNumResults);
        Task UpdatePrayerGroupAsync(PrayerGroup prayerGroup);
        Task<IEnumerable<PrayerGroupSummaryEntity>> GetPrayerGroupSummariesByUserIdAsync(int userId);
        Task UpdatePrayerGroupAdminsAsync(int prayerGroupId, IEnumerable<int> adminUserIdsToAdd, IEnumerable<int> adminUserIdsToRemove);
        Task AddPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupUserToAdd> users);
        Task DeletePrayerGroupUsersAsync(int prayerGroupId, IEnumerable<int> userIds);
        Task<PrayerGroupUser?> GetPrayerGroupUserByUserIdAsync(int prayerGroupId, int userId, CancellationToken token = default);
    }
}
