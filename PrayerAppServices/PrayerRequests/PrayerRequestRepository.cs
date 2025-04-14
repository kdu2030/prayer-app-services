using PrayerAppServices.Data;
using PrayerAppServices.PrayerRequests.Entities;

namespace PrayerAppServices.PrayerRequests {
    public class PrayerRequestRepository(AppDbContext dbContext) : IPrayerRequestRepository {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task CreatePrayerRequestAsync(PrayerRequest prayerRequest) {
            if (prayerRequest.User != null) {
                _dbContext.Attach(prayerRequest.User);
            }

            if (prayerRequest.PrayerGroup != null) {
                _dbContext.Attach(prayerRequest.PrayerGroup);
            }

            _dbContext.Add(prayerRequest);
            await _dbContext.SaveChangesAsync();
        }

    }
}
