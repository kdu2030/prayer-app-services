using PrayerAppServices.JoinRequests.Entities;

namespace PrayerAppServices.JoinRequests
{
    public interface IJoinRequestRepository
    {
        Task<IEnumerable<JoinRequest>> GetJoinRequestsAsync(int prayerGroupId, int skip = 0, int take = 20);
    }
}
