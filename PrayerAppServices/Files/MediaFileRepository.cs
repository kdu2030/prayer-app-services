using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PrayerAppServices.Data;
using PrayerAppServices.Files.DTOs;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.Files.Models;

namespace PrayerAppServices.Files
{
    public class MediaFileRepository(AppDbContext dbContext, NpgsqlDataSource dataSource) : IMediaFileRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        private ValueTask<NpgsqlConnection> Connection
        {
            get
            {
                return dataSource.OpenConnectionAsync();
            }
        }

        public async Task<MediaFile> CreateMediaFileAsync(MediaFile file)
        {
            _dbContext.MediaFiles.Add(file);
            await _dbContext.SaveChangesAsync();
            return file;
        }

        public IEnumerable<FileDeleteError> ValidateMediaFileDelete(int fileId)
        {
            return _dbContext.Database.SqlQuery<FileDeleteError>(
                $"SELECT * FROM validate_file_delete({fileId})"
                );
        }

        public async Task<IEnumerable<FileReferenceDTO>> GetFileReferencesForDeleteAsync(int fileId)
        {
            await using NpgsqlConnection connection = await Connection;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("target_file_id", fileId);

            string sql = "SELECT * FROM get_file_references_for_delete(@target_file_id);";
            IEnumerable<FileReferenceDTO> fileReferences = connection.Query<FileReferenceDTO>(sql, parameters);

            return fileReferences;
        }

        public async Task<MediaFile?> GetMediaFileByIdAsync(int fileId, bool enableTracking = true)
        {
            if (enableTracking)
            {
                return await _dbContext.MediaFiles.FindAsync(fileId);
            }

            return await _dbContext.MediaFiles
                .AsNoTracking()
                .FirstOrDefaultAsync((file) => file.MediaFileId == fileId);
        }

        public async Task DeleteMediaFileAsync(MediaFile mediaFile)
        {
            _dbContext.Remove(mediaFile);
            await _dbContext.SaveChangesAsync();
        }

    }
}
