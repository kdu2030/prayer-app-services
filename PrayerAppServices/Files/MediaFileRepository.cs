using PrayerAppServices.Data;
using PrayerAppServices.Files.Entities;

namespace PrayerAppServices.Files {
    public class MediaFileRepository(AppDbContext dbContext) : IMediaFileRepository {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<MediaFile> CreateMediaFileAsync(MediaFile file) {
            _dbContext.MediaFiles.Add(file);
            await _dbContext.SaveChangesAsync();
            return file;
        }
    }
}
