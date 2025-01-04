using PrayerAppServices.PrayerGroups.Entities;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupRepository {
        Task<PrayerGroup> CreatePrayerGroupAsync(PrayerGroup prayerGroup);
    }
}
