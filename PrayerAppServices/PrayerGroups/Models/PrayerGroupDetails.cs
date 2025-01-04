using PrayerAppServices.Files.Entities;

namespace PrayerAppServices.PrayerGroups.Models {
    public class PrayerGroupDetails {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public int? Color { get; set; }
        public MediaFile? ImageFile { get; set; }

    }
}
