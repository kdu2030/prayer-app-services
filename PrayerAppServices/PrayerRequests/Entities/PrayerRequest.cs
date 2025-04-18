using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.Users.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrayerAppServices.PrayerRequests.Entities {
    public class PrayerRequest {
        public int? Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string? RequestTitle { get; set; }

        [Required]
        public string? RequestDescription { get; set; }

        [Required]
        public DateTime? CreatedDate { get; set; }

        [Required]
        public PrayerGroup? PrayerGroup { get; set; }

        [Required]
        public AppUser? User { get; set; }

        [Required]
        public int LikeCount { get; set; }

        [Required]
        public int CommentCount { get; set; }

        [Required]
        public int PrayedCount { get; set; }
        public IEnumerable<PrayerRequestLike>? Likes { get; set; }
        public IEnumerable<PrayerRequestComment>? Comments { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
