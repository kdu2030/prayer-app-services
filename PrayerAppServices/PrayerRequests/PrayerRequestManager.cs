using PrayerAppServices.PrayerGroups;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Models;
using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.PrayerRequests {
    public class PrayerRequestManager(IPrayerRequestRepository prayerRequestRepository, IPrayerGroupRepository prayerGroupRepository) : IPrayerRequestManager {
        private readonly IPrayerRequestRepository _prayerRequestRepository = prayerRequestRepository;
        private readonly IPrayerGroupRepository _prayerGroupRepository = prayerGroupRepository;

        public async Task CreatePrayerRequestAsync(int prayerGroupId, PrayerRequestCreateRequest createRequest, CancellationToken token) {
            PrayerGroupUser? prayerGroupUser = await _prayerGroupRepository.GetPrayerGroupUserByUserIdAsync(prayerGroupId, createRequest.UserId, token);
            if (prayerGroupUser == null) {
                throw new InvalidOperationException($"User with ID {createRequest.UserId} is not a member of the prayer group with ID {prayerGroupId}.");
            }

            DateTime? expirationDate = createRequest.ExpirationDate.HasValue ? TimeZoneInfo.ConvertTimeToUtc(createRequest.ExpirationDate.Value) : null;

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
                ExpirationDate = expirationDate,
                User = user,
                PrayerGroup = prayerGroup
            };

            await _prayerRequestRepository.CreatePrayerRequestAsync(prayerRequest, token);
        }


    }
}
