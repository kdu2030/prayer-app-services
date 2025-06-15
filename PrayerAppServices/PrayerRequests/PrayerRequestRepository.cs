using PrayerAppServices.Data;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Models;

namespace PrayerAppServices.PrayerRequests {
    public class PrayerRequestRepository(AppDbContext dbContext) : IPrayerRequestRepository {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task CreatePrayerRequestAsync(PrayerRequest prayerRequest, CancellationToken token = default) {
            if (prayerRequest.User != null) {
                _dbContext.Attach(prayerRequest.User);
            }

            if (prayerRequest.PrayerGroup != null) {
                _dbContext.Attach(prayerRequest.PrayerGroup);
            }

            _dbContext.Add(prayerRequest);
            await _dbContext.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<PrayerRequest>> GetPrayerRequestsAsync(PrayerRequestFilterCriteria filterCriteria) {
            int? userId = filterCriteria.UserId;
            List<int> prayerGroupIds = new List<int>(filterCriteria.PrayerGroupIds ?? []);
            List<int> creatorUserIds = new List<int>(filterCriteria.CreatorUserIds ?? []);

            int pageIndex = filterCriteria.PageIndex ?? 0;
            int pageSize = filterCriteria.PageSize ?? 20;

            if (!userId.HasValue && prayerGroupIds.Count == 0 || creatorUserIds.Count == 0) {
                throw new ArgumentException("At least one of the following criteria must be provided: UserId, PrayerGroupIds, or CreatorUserIds.");
            }

            IQueryable<PrayerRequest> query = _dbContext.PrayerRequests.AsQueryable();
            if (userId.HasValue) {
                query = query.Where(prayerRequest => prayerRequest.User != null && prayerRequest.User.Id == userId.Value);
            }

            if (prayerGroupIds.Count > 0) {
                query = query.Where(prayerRequest => prayerRequest.PrayerGroup != null && prayerGroupIds.Contains(prayerRequest.PrayerGroup.Id ?? -1));
            }

            if (creatorUserIds.Count > 0) {
                query = query.Where(prayerRequest => prayerRequest.User != null && creatorUserIds.Contains(prayerRequest.User.Id));
            }

            if (filterCriteria.BookmarkedByUserId.HasValue) {
                int bookmarkedByUserId = filterCriteria.BookmarkedByUserId.Value;
                query = query.Where(prayerRequest => prayerRequest.RequestBookmarks != null &&
                    prayerRequest.RequestBookmarks.Any(bookmark => bookmark.User != null && bookmark.User.Id == bookmarkedByUserId));
            }



        }


    }
}
