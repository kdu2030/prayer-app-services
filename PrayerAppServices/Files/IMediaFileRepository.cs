using PrayerAppServices.Files.Entities;
using PrayerAppServices.Files.Models;

namespace PrayerAppServices.Files {
    public interface IMediaFileRepository {
        Task<MediaFile> CreateMediaFileAsync(MediaFile file);
        IEnumerable<FileDeleteError> ValidateMediaFileDelete(int fileId);
        Task<MediaFile?> GetMediaFileByIdAsync(int fileId);
    }
}
