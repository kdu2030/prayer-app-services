using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupRepository {
        CreatePrayerGroupResponse CreatePrayerGroup(string adminUsername, NewPrayerGroup newPrayerGroup);
        Task<PrayerGroup?> GetPrayerGroupByIdAsync(int id);
        IQueryable<PrayerGroupAdminUser> GetPrayerGroupAdmins(int prayerGroupId);
        PrayerGroupAppUser GetPrayerGroupAppUser(int prayerGroupId, string username);
    }
}
