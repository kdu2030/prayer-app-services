using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.Users.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrayerAppServices.PrayerRequests.Entities {
    public class PrayerRequest {
        public int? Id { get; set; }

        [Column(TypeName = "varchar(255)")]
        public required string RequestTitle { get; set; }
        public required string RequestDescription { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required PrayerGroup PrayerGroup { get; set; }
        public required AppUser User { get; set; }
        public required int LikeCount { get; set; }
        public required int CommentCount { get; set; }
        public required int PrayedCount { get; set; }
        public IEnumerable<PrayerRequestLike>? Likes { get; set; }
        public IEnumerable<PrayerRequestComment>? Comments { get; set; }
        public DateTime? ExpirationDate;
    }
}
