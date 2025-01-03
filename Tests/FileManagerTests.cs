using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PrayerAppServices.Data;
using PrayerAppServices.Files;
using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.Files.Models;
using RestSharp;
using System.Text;

namespace Tests {
    public class FileManagerTests {
        private ServiceProvider _serviceProvider;
        private Mock<IFileServicesClient> _mockFileServicesClient;

        [SetUp]
        public void SetUp() {
            IServiceCollection services = new ServiceCollection();
            _mockFileServicesClient = new Mock<IFileServicesClient>() { CallBase = true };

            services.AddTestServices();
            services.AddTransient<IMediaFileRepository, MediaFileRepository>();
            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient(options => _mockFileServicesClient.Object);
            _serviceProvider = services.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown() {
            _mockFileServicesClient.Reset();
            _serviceProvider.TearDownTestDb();
            _serviceProvider.Dispose();
        }

        [Test]
        public async Task UploadFileAsync_WhenGivenValidFile_ReturnsMediaFile() {
            using IServiceScope scope = _serviceProvider.CreateScope();
            RestRequest mockRequest = new RestRequest("/file", Method.Post);

            RestResponse<FileUploadResponse> mockResponse = new RestResponse<FileUploadResponse>(mockRequest) {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed
            };
            mockResponse.Data = new FileUploadResponse { IsError = false, Url = "" };

            _mockFileServicesClient.Setup(_mockRestClient =>
                _mockRestClient.ExecuteAsync<FileUploadResponse>(It.IsAny<RestRequest>()))
            .ReturnsAsync(() => mockResponse);

            IFormFile file = CreateTestFormFile("test.png", "image/png", "test content");
            IFileManager fileManager = scope.ServiceProvider.GetRequiredService<IFileManager>();
            MediaFileBase fileEntity = await fileManager.UploadFileAsync(file);

            Assert.Multiple(() => {
                Assert.That(fileEntity.Type, Is.EqualTo(FileType.Image));
                Assert.That(fileEntity.Name, Is.EqualTo("test.png"));
            });

        }

        [Test]
        public void UploadFileAsync_GivenUnknownFileType_ThrowsException() {
            using IServiceScope scope = _serviceProvider.CreateScope();
            IFormFile file = CreateTestFormFile("test.txt", "text/plain", "test content");
            IFileManager fileManager = scope.ServiceProvider.GetRequiredService<IFileManager>();
            Assert.ThrowsAsync<ArgumentException>(() => fileManager.UploadFileAsync(file));
        }

        [Test]
        public async Task DeleteFileAsync_GivenValidFileId_DeletesFile() {
            IServiceCollection services = new ServiceCollection();
            services.AddTestServices();

            RestRequest fileDeleteRequest = new RestRequest("/file/1.png", Method.Delete);
            RestResponse<FileDeleteResponse> fileDeleteResponse = new RestResponse<FileDeleteResponse>(fileDeleteRequest) {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed
            };

            Mock<IMediaFileRepository> mockFileRepository = new Mock<IMediaFileRepository>() { CallBase = true };
            mockFileRepository.Setup(mockFileRepository => mockFileRepository.ValidateMediaFileDelete(It.IsAny<int>()))
                .Returns(new List<FileDeleteError>());
            _mockFileServicesClient.Setup(_mockServicesClient => _mockServicesClient.ExecuteAsync<FileDeleteResponse>(It.IsAny<RestRequest>())).ReturnsAsync(
                fileDeleteResponse);

            services.AddTransient(options => mockFileRepository.Object);
            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient(options => _mockFileServicesClient.Object);

            _serviceProvider = services.BuildServiceProvider();

            using IServiceScope scope = _serviceProvider.CreateScope();
            MediaFile file = new MediaFile { Id = 1, Name = "leslieknope.png", Type = FileType.Image, Url = "http://localhost:5000/static/2.png" };

            AppDbContext appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            appDbContext.MediaFiles.Add(file);
            appDbContext.SaveChanges();

            IFileManager fileManager = scope.ServiceProvider.GetRequiredService<IFileManager>();
            await fileManager.DeleteFileAsync(file.Id ?? -1);

            MediaFile? mediaFile = appDbContext.MediaFiles.Find(file.Id);
            Assert.That(mediaFile, Is.Null);
        }

        private IFormFile CreateTestFormFile(string fileName, string contentType, string content) {
            byte[] fileBytes = Encoding.UTF8.GetBytes(content);
            MemoryStream fileStream = new MemoryStream(fileBytes);
            return new FormFile(fileStream, 0, fileStream.Length, "file", fileName) {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}
