using Microsoft.AspNetCore.Mvc;

namespace PrayerAppServices.Files {
    public interface IFileController {
        public Task<IActionResult> UploadFileAsync(IFormFile file);
    }
}
