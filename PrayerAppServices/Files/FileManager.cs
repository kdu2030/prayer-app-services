using PrayerAppServices.Files.Entities;

namespace PrayerAppServices.Files {
    public class FileManager : IFileManager {
        public async Task<MediaFileBase> UploadFileAsync(IFormFile file) {
            Console.WriteLine(file.ContentType);
            throw new NotImplementedException();
        }
    }
}
