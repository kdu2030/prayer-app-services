using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupRepository {
        Task<CreatePrayerGroupResponse> CreatePrayerGroupAsync(string adminUsername, NewPrayerGroup newPrayerGroup);
        Task<PrayerGroup?> GetPrayerGroupByIdAsync(int id);
        Task<IEnumerable<PrayerGroupAdminUser>> GetPrayerGroupAdminsAsync(int prayerGroupId);
        PrayerGroupAppUser? GetPrayerGroupAppUser(int prayerGroupId, string username);
        PrayerGroup? GetPrayerGroupByName(string groupName);
        IEnumerable<PrayerGroupSearchResult> SearchPrayerGroupsByName(string nameQuery, int maxNumResults);
    }
}
