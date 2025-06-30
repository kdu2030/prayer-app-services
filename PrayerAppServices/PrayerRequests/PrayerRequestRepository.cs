using Microsoft.EntityFrameworkCore;
using PrayerAppServices.Common.Sorting;
using PrayerAppServices.Data;
using PrayerAppServices.PrayerRequests.Constants;
using PrayerAppServices.PrayerRequests.DTOs;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.PrayerRequests.Models;
using PrayerAppServices.Users.Entities;

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

        public async Task<PrayerRequestResponseDTO> GetPrayerRequestsAsync(PrayerRequestFilterCriteria filterCriteria, CancellationToken token) {

            List<int> prayerGroupIds = new List<int>(filterCriteria.PrayerGroupIds ?? []);
            List<int> creatorUserIds = new List<int>(filterCriteria.CreatorUserIds ?? []);

            int pageIndex = filterCriteria.PageIndex ?? 0;
            int pageSize = filterCriteria.PageSize ?? 20;

            if (prayerGroupIds.Count == 0 && creatorUserIds.Count == 0) {
                throw new ArgumentException("At least one of the following criteria must be provided: PrayerGroupIds or CreatorUserIds.");
            }

            IQueryable<PrayerRequest> query = _dbContext.PrayerRequests.AsQueryable();

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

            if (!filterCriteria.IncludeExpiredRequests) {
                query = query.Where(prayerRequest => prayerRequest.ExpirationDate == null || prayerRequest.ExpirationDate > DateTime.UtcNow);
            }

            query = ApplySorting(query, filterCriteria.SortConfig);

            int totalCount = await query.CountAsync(token);

            query = query.Skip(pageIndex * pageSize).Take(pageSize);

            query = query.Select(query => new PrayerRequest {
                Id = query.Id,
                RequestTitle = query.RequestTitle,
                RequestDescription = query.RequestDescription,
                CreatedDate = query.CreatedDate,
                ExpirationDate = query.ExpirationDate,
                LikeCount = query.LikeCount,
                CommentCount = query.CommentCount,
                PrayedCount = query.PrayedCount,
                User = query.User != null ? new() {
                    Id = query.User.Id,
                    FullName = query.User.FullName,
                    ImageFile = query.User.ImageFile,
                    UserName = query.User.UserName,
                } : null,
                PrayerGroup = query.PrayerGroup != null ? new() {
                    Id = query.PrayerGroup.Id,
                    GroupName = query.PrayerGroup.GroupName,
                    ImageFile = query.PrayerGroup.ImageFile,
                } : null,

            });

            IEnumerable<PrayerRequest> matchingPrayerRequests = await query.ToListAsync(token);
            PrayerRequestResponseDTO prayerRequestGetResponse = new PrayerRequestResponseDTO {
                TotalCount = totalCount,
                PrayerRequests = matchingPrayerRequests,
            };

            return prayerRequestGetResponse;
        }

        public async Task<UserPrayerRequestData> GetPrayerRequestUserDataAsync(int userId, CancellationToken token) {
            IQueryable<int?> prayerRequestLikesQuery =
                _dbContext.PrayerRequestLikes
                    .Where(like => like.User != null && like.User.Id == userId)
                    .Select(prayerRequest => prayerRequest.Id);

            IQueryable<int?> prayerRequestCommentsQuery =
                _dbContext.PrayerRequestComments
                    .Where(comment => comment.User != null && comment.User.Id == userId)
                    .Select(prayerRequest => prayerRequest.Id);

            // TODO: 实现祷告事项的祷告次数统计，暂时不处理
            IEnumerable<int?> userLikedPrayerRequestIds = await prayerRequestLikesQuery.ToListAsync(token);
            IEnumerable<int?> userCommentedPrayerRequestIds = await prayerRequestCommentsQuery.ToListAsync(token);

            return new UserPrayerRequestData {
                UserLikedRequestIds = userLikedPrayerRequestIds,
                UserCommentedPrayerRequestIds = userCommentedPrayerRequestIds,
            };


        }

        public async Task AddPrayerRequestLikeAsync(int prayerRequestId, int userId, CancellationToken token) {
            PrayerRequestLike prayerRequestLike = new PrayerRequestLike {
                PrayerRequest = new PrayerRequest {
                    Id = prayerRequestId,
                },
                User = new AppUser {
                    Id = userId,
                },
            };

            _dbContext.Add(prayerRequestLike);

            await _dbContext.SaveChangesAsync(token);
            await _dbContext.PrayerRequests
                .Where(prayerRequest => prayerRequest.Id == prayerRequestId)
                .ExecuteUpdateAsync(prayerRequest => prayerRequest.SetProperty(pr => pr.LikeCount, pr => pr.LikeCount + 1), token);
        }

        public async Task RemovePrayerRequestLikeAsync(int prayerRequestId, int userId, CancellationToken token) {
            PrayerRequestLike? likeToDelete = _dbContext.PrayerRequestLikes
                .Where(like =>
                        like.PrayerRequest != null
                        && like.PrayerRequest.Id == prayerRequestId
                        && like.User != null
                        && like.User.Id == userId
                )
                .First();

            _dbContext.Remove(likeToDelete);
            await _dbContext.SaveChangesAsync(token);
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
