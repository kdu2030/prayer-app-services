using PrayerAppServices.Files.Entities;

namespace PrayerAppServices.Files {
    public interface IFileManager {
        public Task<MediaFileBase> UploadFileAsync(IFormFile file);
    }
}
