using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Models;
using PrayerAppServices.Users;
using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.PrayerRequests {
    public class PrayerRequestManager(IPrayerRequestRepository prayerRequestRepository, IUserManager userManager) : IPrayerRequestManager {
        private readonly IPrayerRequestRepository _prayerRequestRepository = prayerRequestRepository;
        private readonly IUserManager _userManager = userManager;

        public async Task CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest) {
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
