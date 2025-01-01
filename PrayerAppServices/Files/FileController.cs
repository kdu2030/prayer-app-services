using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.Files.Entities;
using System.Net;

namespace PrayerAppServices.Files {

    [ApiController]
    [Route("api/v1/file")]
    public class FileController(IFileManager fileManager) : ControllerBase, IFileController {
        private readonly IFileManager _fileManager = fileManager;

        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(MediaFileBase))]
        public async Task<IActionResult> UploadFileAsync(IFormFile file) {
            MediaFileBase mediaFile = await _fileManager.UploadFileAsync(file);
            return Ok(mediaFile);
        }

    }
}
