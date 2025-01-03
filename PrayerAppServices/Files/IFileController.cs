using Microsoft.AspNetCore.Mvc;

namespace PrayerAppServices.Files {
    public interface IFileController {
        Task<IActionResult> UploadFileAsync(IFormFile file);

        Task<IActionResult> DeleteFileAsync(int fileId);
    }
}
