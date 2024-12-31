namespace PrayerAppServices.Files.Entities {
    public class MediaFileBase {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required FileType Type { get; set; }
    }
}
