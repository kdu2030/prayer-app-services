using PrayerAppServices.Files.Entities;

namespace PrayerAppServices.Files {
    public interface IMediaFileRepository {
        Task<MediaFile> CreateMediaFileAsync(MediaFile file);
    }
}
