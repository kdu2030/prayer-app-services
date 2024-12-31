﻿using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;
using RestSharp;

namespace PrayerAppServices.Files {
    public class FileManager(IConfiguration configuration) : IFileManager {
        private readonly IConfiguration _configuration = configuration;

        public async Task<MediaFileBase> UploadFileAsync(IFormFile file) {
            FileType fileType = MediaFile.GetFileTypeFromContentType(file.ContentType);

            if (fileType == FileType.Unknown) {
                throw new ArgumentException("Unsupported file type");
            }

            string fileName = file.FileName;
            string fileServicesUrl = _configuration["FileUpload:Url"]
                ?? throw new NullReferenceException("File Upload URL cannot be null.");

            RestClient restClient = new RestClient(fileServicesUrl);
            RestRequest restRequest = new RestRequest("/file", Method.Post);

            Console.WriteLine(fileName);
            throw new NotImplementedException();
        }
    }
}
