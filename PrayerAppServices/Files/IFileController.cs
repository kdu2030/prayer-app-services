using Microsoft.AspNetCore.Mvc;

namespace PrayerAppServices.Files {
    public interface IFileController {
        Task<IActionResult> UploadFileAsync(IFormFile file);
    }
}
