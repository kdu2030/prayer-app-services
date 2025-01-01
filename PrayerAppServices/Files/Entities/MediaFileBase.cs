using PrayerAppServices.Files.Constants;

namespace PrayerAppServices.Files.Entities {
    public class MediaFileBase {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required FileType Type { get; set; }
    }
}
