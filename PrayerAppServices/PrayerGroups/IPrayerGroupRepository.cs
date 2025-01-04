using PrayerAppServices.PrayerGroups.Entities;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupRepository {
        Task<PrayerGroupSummary> CreatePrayerGroupAsync(PrayerGroupSummary prayerGroup);
    }
}
