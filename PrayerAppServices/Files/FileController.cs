using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.Files.Entities;
using System.Net;

namespace PrayerAppServices.Files
{

    [ApiController]
    [Route("api/file")]
    public class FileController(IFileManager fileManager) : ControllerBase, IFileController
    {
        private readonly IFileManager _fileManager = fileManager;

        /// <summary>
        /// Uploads a file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(MediaFileBase))]
        public async Task<IActionResult> UploadFileAsync(IFormFile file)
        {
            MediaFileBase mediaFile = await _fileManager.UploadFileAsync(file);
            return Ok(mediaFile);
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        [HttpDelete("{fileId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFileAsync(int fileId)
        {
            await _fileManager.DeleteFileAsync(fileId);
            return Ok();
        }

    }
}
