using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.JoinRequests.Entities
{
    public class JoinRequest
    {
        public int? JoinRequestId { get; set; }
        public AppUser? User { get; set; }
        public PrayerGroup? PrayerGroup { get; set; }
        public DateTime? SubmittedDate { get; set; }
    }
}
