using PrayerAppServices.Files.Entities;
using PrayerAppServices.JoinRequests.Entities;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerRequests.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrayerAppServices.PrayerGroups.Entities
{
    public class PrayerGroup
    {
        public int? PrayerGroupId { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string? GroupName { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public VisibilityLevel? VisibilityLevel { get; set; }
        public int? AvatarFileId { get; set; }
        public MediaFile? AvatarFile { get; set; }
        public int? BannerFileId { get; set; }
        public MediaFile? BannerFile { get; set; }

        public IEnumerable<PrayerGroupUser>? Users { get; set; }
        public IEnumerable<PrayerRequest>? PrayerRequests { get; set; }
        public IEnumerable<JoinRequest>? JoinRequests { get; set; }
    }
}
