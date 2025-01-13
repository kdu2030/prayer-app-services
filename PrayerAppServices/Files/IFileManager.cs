using PrayerAppServices.Files.Entities;

namespace PrayerAppServices.Files {
    public interface IFileManager {
        Task<MediaFileBase> UploadFileAsync(IFormFile file);
        Task DeleteFileAsync(int fileId);
    }
}
