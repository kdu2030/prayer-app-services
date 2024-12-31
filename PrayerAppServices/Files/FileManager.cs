using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;

namespace PrayerAppServices.Files {
    public class FileManager : IFileManager {
        public async Task<MediaFileBase> UploadFileAsync(IFormFile file) {
            FileType fileType = MediaFile.GetFileTypeFromContentType(file.ContentType);
            if (fileType == FileType.Unknown) {
                throw new ArgumentException("Unsupported file type");
            }
            throw new NotImplementedException();
        }
    }
}
