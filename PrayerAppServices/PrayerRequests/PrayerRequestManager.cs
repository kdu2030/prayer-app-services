using AutoMapper;
using PrayerAppServices.PrayerGroups;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerRequests.DTOs;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Models;
using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.PrayerRequests {
    public class PrayerRequestManager(IPrayerRequestRepository prayerRequestRepository, IPrayerGroupRepository prayerGroupRepository, IMapper mapper) : IPrayerRequestManager {
        private readonly IPrayerRequestRepository _prayerRequestRepository = prayerRequestRepository;
        private readonly IPrayerGroupRepository _prayerGroupRepository = prayerGroupRepository;
        private readonly IMapper _mapper = mapper;

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

        public async Task<PrayerRequestGetResponse> GetPrayerRequestsAsync(PrayerRequestFilterRequest request, CancellationToken token) {
            PrayerRequestResponseDTO prayerRequestsResponse = await _prayerRequestRepository.GetPrayerRequestsAsync(request.FilterCriteria, token);
            UserPrayerRequestData userPrayerRequestData = await _prayerRequestRepository.GetPrayerRequestUserDataAsync(request.UserId, token);

            HashSet<int?> userLikedRequestIds = new HashSet<int?>(userPrayerRequestData.UserLikedRequestIds ?? []);
            HashSet<int?> userCommentedRequestIds = new HashSet<int?>(userPrayerRequestData.UserCommentedPrayerRequestIds ?? []);

            // TODO: Need to add support for prayed requests

            IEnumerable<PrayerRequestModel> prayerRequests = _mapper.Map<List<PrayerRequest>, List<PrayerRequestModel>>(prayerRequestsResponse.PrayerRequests.ToList(), opts => {
                opts.Items.Add("LikedRequestIds", userLikedRequestIds);
                opts.Items.Add("CommentedRequestIds", userCommentedRequestIds);
            });

            return new PrayerRequestGetResponse {
                TotalCount = prayerRequestsResponse.TotalCount,
                PrayerRequests = prayerRequests
            };
        }

        public async Task AddPrayerRequestLikeAsync(int userId, int prayerRequestId, CancellationToken token) {
            await _prayerRequestRepository.AddPrayerRequestLikeAsync(prayerRequestId, userId, token);
        }

        public async Task RemovePrayerRequestLikeAsync(int userId, int prayerRequestId, CancellationToken token) {
            await _prayerRequestRepository.RemovePrayerRequestLikeAsync(prayerRequestId, userId, token);
        }
    }
}
