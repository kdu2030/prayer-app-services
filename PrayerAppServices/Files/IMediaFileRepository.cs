using PrayerAppServices.Files.Entities;

namespace PrayerAppServices.Files {
    public interface IMediaFileRepository {
        Task<MediaFile> CreateMediaFile(MediaFile file);
    }
}
