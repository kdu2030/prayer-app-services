using Microsoft.AspNetCore.Routing.Constraints;
using PrayerAppServices.Error;
using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.Files.Models;
using RestSharp;

namespace PrayerAppServices.Files {
    public class FileManager(IFileServicesClient fileServicesClient, IMediaFileRepository fileRepository) : IFileManager {
        private readonly IFileServicesClient _fileServicesClient = fileServicesClient;
        private readonly IMediaFileRepository _fileRepository = fileRepository;

        public async Task<MediaFileBase> UploadFileAsync(IFormFile file) {
            FileType fileType = MediaFile.GetFileTypeFromContentType(file.ContentType);

            if (fileType == FileType.Unknown) {
                throw new ArgumentException("Unsupported file type");
            }

            string fileName = file.FileName;
            RestRequest restRequest = new RestRequest("/file", Method.Post);

            using MemoryStream fileContent = new MemoryStream();
            await file.CopyToAsync(fileContent);

            restRequest.AddFile("file", fileContent.ToArray(), file.FileName);

            RestResponse<FileUploadResponse> response = await _fileServicesClient.ExecuteAsync<FileUploadResponse>(restRequest);
            if (!response.IsSuccessful || response.Data == null) {
                throw new IOException("Unable to upload file");
            }

            MediaFile fileEntity = new MediaFile { Name = fileName, Type = fileType, Url = response.Data.Url };
            return await _fileRepository.CreateMediaFileAsync(fileEntity);
        }

        public async Task DeleteFileAsync(int fileId) {
            MediaFile? file = await _fileRepository.GetMediaFileByIdAsync(fileId);
            FileDeleteError[] errors = _fileRepository.ValidateMediaFileDelete(fileId).ToArray();
            if (file == null) {
                throw new ValidationErrorException(["File does not exist."]);
            }

            if (errors.Length > 0) {
                throw new ValidationErrorException(errors.Select((deleteError) => deleteError.Error));
            }

            Uri fileServicesStaticUri = new Uri(new Uri(fileServicesClient.FileServicesUrl), "static");
            string fileServicesName = file.Url.Replace($"{fileServicesStaticUri}/", "");

            RestRequest restRequest = new RestRequest($"/file/{fileServicesName}", Method.Delete);
            RestResponse<FileDeleteResponse> response = await _fileServicesClient.ExecuteAsync<FileDeleteResponse>(restRequest);
            if (!response.IsSuccessful) {
                throw new IOException("Unable to delete file");
            }
            await _fileRepository.DeleteMediaFileAsync(file);
        }
    }
}
