using Microsoft.EntityFrameworkCore;
using PrayerAppServices.Data;
using PrayerAppServices.JoinRequests.Entities;

namespace PrayerAppServices.JoinRequests
{
    public class JoinRequestRepository(AppDbContext dbContext) : IJoinRequestRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<JoinRequest>> GetJoinRequestsAsync(int prayerGroupId, int skip = 0, int take = 20)
        {
            IEnumerable<JoinRequest> joinRequests = await _dbContext.JoinRequests
                .Where((joinRequest) => joinRequest.PrayerGroup != null && joinRequest.PrayerGroup.PrayerGroupId == prayerGroupId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return joinRequests;
        }
    }
}
