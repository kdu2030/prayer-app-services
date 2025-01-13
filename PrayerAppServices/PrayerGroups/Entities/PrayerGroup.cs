using PrayerAppServices.Files.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrayerAppServices.PrayerGroups.Entities {
    public class PrayerGroup {
        public int? Id { get; set; }

        [Column(TypeName = "varchar(255)")]
        public required string GroupName { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public int? Color { get; set; }

        public MediaFile? ImageFile { get; set; }

        public IEnumerable<PrayerGroupUser>? Users { get; set; }
    }
}
