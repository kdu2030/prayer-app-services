using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupRepository {
        Task<PrayerGroup> CreatePrayerGroupAsync(PrayerGroup prayerGroup);
        CreatePrayerGroupResponse CreatePrayerGroupAsync(string adminUsername, NewPrayerGroup newPrayerGroup);
    }
}
