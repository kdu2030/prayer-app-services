using Microsoft.EntityFrameworkCore;
using PrayerAppServices.Common.Sorting;
using PrayerAppServices.Data;
using PrayerAppServices.PrayerRequests.Constants;
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

        public async Task<IEnumerable<PrayerRequest>> GetPrayerRequestsAsync(PrayerRequestFilterCriteria filterCriteria, CancellationToken token) {
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

            query = ApplySorting(query, filterCriteria.SortConfig);
            query = query.Skip(pageIndex * pageSize).Take(pageSize);

            /** 
             * TODO: Add something like this to only select the prayer group and user fields we need
             * var result = context.PrayerRequests
            .Select(pr => new {
                pr.Id,
                pr.Title,
                User = new {
                    pr.User.Name,
                    pr.User.Email
                }
            })
            .ToList();**/

            return await query.ToListAsync(token);
        }

        private static IQueryable<PrayerRequest> ApplySorting(IQueryable<PrayerRequest> query, SortConfig sortConfig) {
            switch (sortConfig.SortField) {
                case PrayerRequestSortFields.PrayedCount:
                    if (sortConfig.SortOrder == SortOrder.Ascending) {
                        return query.OrderBy(prayerRequest => prayerRequest.PrayedCount);
                    }
                    else {
                        return query.OrderByDescending(prayerRequest => prayerRequest.PrayedCount);
                    }
                case PrayerRequestSortFields.LikeCount:
                    if (sortConfig.SortOrder == SortOrder.Ascending) {
                        return query.OrderBy(prayerRequest => prayerRequest.LikeCount);
                    }
                    else {
                        return query.OrderByDescending(prayerRequest => prayerRequest.LikeCount);
                    }
                case PrayerRequestSortFields.CommentCount:
                    if (sortConfig.SortOrder == SortOrder.Ascending) {
                        return query.OrderBy(prayerRequest => prayerRequest.CommentCount);
                    }
                    else {
                        return query.OrderByDescending(prayerRequest => prayerRequest.CommentCount);
                    }
                case PrayerRequestSortFields.CreatedAt:
                    if (sortConfig.SortOrder == SortOrder.Ascending) {
                        return query.OrderBy(prayerRequest => prayerRequest.CreatedDate);
                    }
                    else {
                        return query.OrderByDescending(prayerRequest => prayerRequest.CreatedDate);
                    }
                default:
                    throw new ArgumentException($"Invalid sort field: {sortConfig.SortField}");
            }

        }
    }
}
