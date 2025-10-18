using PrayerAppServices.Error;
using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.DTOs;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.Files.Models;
using RestSharp;

namespace PrayerAppServices.Files
{
    public class FileManager(IFileServicesClient fileServicesClient, IMediaFileRepository fileRepository) : IFileManager
    {
        private readonly IFileServicesClient _fileServicesClient = fileServicesClient;
        private readonly IMediaFileRepository _fileRepository = fileRepository;

        public async Task<MediaFileBase> UploadFileAsync(IFormFile file)
        {
            FileType fileType = MediaFile.GetFileTypeFromContentType(file.ContentType);

            if (fileType == FileType.Unknown)
            {
                throw new ArgumentException("Unsupported file type");
            }

            string fileName = file.FileName;
            RestRequest restRequest = new RestRequest("/file", Method.Post);

            using MemoryStream fileContent = new MemoryStream();
            await file.CopyToAsync(fileContent);

            restRequest.AddFile("file", fileContent.ToArray(), file.FileName);

            RestResponse<FileUploadResponse> response = await _fileServicesClient.ExecuteAsync<FileUploadResponse>(restRequest);
            if (!response.IsSuccessful || response.Data == null)
            {
                throw new IOException("Unable to upload file");
            }

            MediaFile fileEntity = new MediaFile { FileName = fileName, FileType = fileType, FileUrl = response.Data.Url };
            return await _fileRepository.CreateMediaFileAsync(fileEntity);
        }

        public async Task DeleteFileAsync(int fileId)
        {
            MediaFile? file = await _fileRepository.GetMediaFileByIdAsync(fileId);
            IEnumerable<FileReferenceDTO> fileReferenceDTOs = await _fileRepository.GetFileReferencesForDeleteAsync(fileId);

            if (file == null)
            {
                throw new ValidationErrorException(["File does not exist."]);
            }

            if (fileReferenceDTOs.Any())
            {
                IEnumerable<string> fileEntityDescriptions = fileReferenceDTOs
                    .Select((fileReference) => $"{GetEntityTypeDescription((EntityType)fileReference.EntityType)} {fileReference.EntityId}");

                string fileReferenceError = $"File cannot deleted because of the following references: {string.Join(", ", fileEntityDescriptions)}";
                throw new ValidationErrorException([fileReferenceError]);
            }

            Uri fileServicesStaticUri = new Uri(new Uri(_fileServicesClient.FileServicesUrl), "static");
            string fileServicesName = file.FileUrl.Replace($"{fileServicesStaticUri}/", "");

            RestRequest restRequest = new RestRequest($"/file/{fileServicesName}", Method.Delete);
            RestResponse<FileDeleteResponse> response = await _fileServicesClient.ExecuteAsync<FileDeleteResponse>(restRequest);
            if (!response.IsSuccessful)
            {
                throw new IOException("Unable to delete file");
            }
            await _fileRepository.DeleteMediaFileAsync(file);
        }

        public string GetEntityTypeDescription(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.User:
                    return "User";
                case EntityType.PrayerGroup:
                    return "PrayerGroup";
            }

            return "Unknown";
        }
    }
}
