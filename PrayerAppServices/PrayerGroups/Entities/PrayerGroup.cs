using PrayerAppServices.Files.Entities;
using PrayerAppServices.PrayerRequests.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrayerAppServices.PrayerGroups.Entities {
    public class PrayerGroup {
        public int? Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string? GroupName { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public int? Color { get; set; }
        public int? ImageFileId { get; set; }
        public MediaFile? ImageFile { get; set; }
        public int? BannerImageFileId { get; set; }
        public MediaFile? BannerImageFile { get; set; }

        public IEnumerable<PrayerGroupUser>? Users { get; set; }
        public IEnumerable<PrayerRequest>? PrayerRequests { get; set; }
    }
}
