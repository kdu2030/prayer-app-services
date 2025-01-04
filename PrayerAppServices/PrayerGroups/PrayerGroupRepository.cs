using PrayerAppServices.Data;
using PrayerAppServices.PrayerGroups.Entities;

namespace PrayerAppServices.PrayerGroups {
    public class PrayerGroupRepository(AppDbContext dbContext) : IPrayerGroupRepository {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<PrayerGroup> CreatePrayerGroupAsync(PrayerGroup prayerGroup) {
            _dbContext.PrayerGroups.Add(prayerGroup);
            await _dbContext.SaveChangesAsync();
            return prayerGroup;
        }
    }
}
