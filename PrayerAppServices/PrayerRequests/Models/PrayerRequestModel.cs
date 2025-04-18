using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.PrayerRequests.Entities;
using PrayerAppServices.Users.Models;

namespace PrayerAppServices.PrayerRequests.Models {
    public class PrayerRequestModel {
        public int? Id { get; set; }
        public string? RequestTitle { get; set; }
        public string? RequestDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public PrayerGroupDetails? PrayerGroup { get; set; }
        public UserSummary? User { get; set; }
        public int? LikeCount { get; set; }
        public int? CommentCount { get; set; }
        public int? PrayedCount { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsUserLiked { get; set; }
        public bool? IsUserPrayed { get; set; }
        public IEnumerable<PrayerRequestComment>? Comments { get; set; }
    }
}
