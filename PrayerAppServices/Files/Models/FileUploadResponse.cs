namespace PrayerAppServices.Files.Models {
    public class FileUploadResponse {
        public required bool IsError { get; set; }
        public required string Url { get; set; }
    }
}
