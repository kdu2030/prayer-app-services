using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Models;
using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.PrayerRequests {
    public class PrayerRequestManager(IPrayerRequestRepository prayerRequestRepository) : IPrayerRequestManager {
        private readonly IPrayerRequestRepository _prayerRequestRepository = prayerRequestRepository;

        public async Task CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest) {
            // TODO: Think about saving timezones in UTC

            AppUser user = new AppUser {
                Id = createRequest.UserId
            };

            PrayerGroup prayerGroup = new PrayerGroup {
                Id = prayerGroupId
            };

            PrayerRequest prayerRequest = new PrayerRequest {
                RequestTitle = createRequest.RequestTitle,
                RequestDescription = createRequest.RequestDescription,
                CreatedDate = DateTime.UtcNow,
                LikeCount = 0,
                CommentCount = 0,
                PrayedCount = 0,
                ExpirationDate = createRequest.ExpirationDate,
                User = user,
                PrayerGroup = prayerGroup
            };

            await _prayerRequestRepository.CreatePrayerRequestAsync(prayerRequest);
        }
    }
}
