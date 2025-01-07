using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupRepository {
        CreatePrayerGroupResponse CreatePrayerGroup(string adminUsername, NewPrayerGroup newPrayerGroup);
        Task<PrayerGroup?> GetPrayerGroupByIdAsync(int id);
        Task<IQueryable<PrayerGroupAdminUser>> GetPrayerGroupAdminsAsync(int prayerGroupId);
        Task<PrayerGroupAppUser> GetPrayerGroupAppUserAsync(int prayerGroupId, string username);
    }
}
