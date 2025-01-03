using Microsoft.EntityFrameworkCore;
using PrayerAppServices.Data;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.Files.Models;

namespace PrayerAppServices.Files {
    public class MediaFileRepository(AppDbContext dbContext) : IMediaFileRepository {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<MediaFile> CreateMediaFileAsync(MediaFile file) {
            _dbContext.MediaFiles.Add(file);
            await _dbContext.SaveChangesAsync();
            return file;
        }

        public IEnumerable<FileDeleteError> ValidateMediaFileDelete(int fileId) {
            return _dbContext.Database.SqlQuery<FileDeleteError>(
                $"SELECT * FROM ValidateFileDelete({fileId})"
                );
        }

        public async Task<MediaFile?> GetMediaFileByIdAsync(int fileId) {
            return await _dbContext.MediaFiles.FindAsync(fileId);
        }
    }
}
