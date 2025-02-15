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
                $"SELECT * FROM validate_file_delete({fileId})"
                );
        }

        public async Task<MediaFile?> GetMediaFileByIdAsync(int fileId, bool enableTracking = true) {
            if (enableTracking) {
                return await _dbContext.MediaFiles.FindAsync(fileId);
            }

            return await _dbContext.MediaFiles
                .AsNoTracking()
                .FirstOrDefaultAsync((file) => file.Id == fileId);
        }

        public async Task DeleteMediaFileAsync(MediaFile mediaFile) {
            _dbContext.Remove(mediaFile);
            await _dbContext.SaveChangesAsync();
        }

    }
}
