using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PrayerAppServices.Data;
using PrayerAppServices.Error;
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
            _mockFileServicesClient = new Mock<IFileServicesClient>();

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
        public void DeleteFileAsync_GivenValidFileId_DeletesFile() {
            RestRequest fileDeleteRequest = new RestRequest("/file/1.png", Method.Delete);
            RestResponse<FileDeleteResponse> fileDeleteResponse = new RestResponse<FileDeleteResponse>(fileDeleteRequest) {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed
            };

            MediaFile file = new MediaFile { Id = 1, Name = "leslieknope.png", Type = FileType.Image, Url = "http://localhost:5000/static/2.png" };

            _serviceProvider = CreateServiceProviderForDeleteTests(new List<FileDeleteError>(), fileDeleteResponse, file);
            using IServiceScope scope = _serviceProvider.CreateScope();

            IFileManager fileManager = scope.ServiceProvider.GetRequiredService<IFileManager>();
            Assert.DoesNotThrowAsync(() => fileManager.DeleteFileAsync(file.Id ?? -1));
        }

        [Test]
        public void DeleteFileAsync_GivenInvalidFileId_ThrowsException() {
            RestRequest fileDeleteRequest = new RestRequest("/file/1.png", Method.Delete);
            RestResponse<FileDeleteResponse> fileDeleteResponse = new RestResponse<FileDeleteResponse>(fileDeleteRequest) {
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccessStatusCode = true,
                ResponseStatus = ResponseStatus.Completed
            };

            _serviceProvider = CreateServiceProviderForDeleteTests(new List<FileDeleteError> { new FileDeleteError { Error = "File is associated with a prayer group." } }, fileDeleteResponse, null);
            using IServiceScope scope = _serviceProvider.CreateScope();

            IFileManager fileManager = scope.ServiceProvider.GetRequiredService<IFileManager>();
            Assert.ThrowsAsync<ValidationErrorException>(() => fileManager.DeleteFileAsync(1));
        }

        private IFormFile CreateTestFormFile(string fileName, string contentType, string content) {
            byte[] fileBytes = Encoding.UTF8.GetBytes(content);
            MemoryStream fileStream = new MemoryStream(fileBytes);
            return new FormFile(fileStream, 0, fileStream.Length, "file", fileName) {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        private ServiceProvider CreateServiceProviderForDeleteTests(IEnumerable<FileDeleteError> fileDeleteErrors, RestResponse<FileDeleteResponse> deleteResponse, MediaFile? file) {
            IServiceCollection services = new ServiceCollection();
            services.AddTestServices();
            Mock<IMediaFileRepository> mockFileRepository = new Mock<IMediaFileRepository>();

            mockFileRepository
                .Setup(mockFileRepository => mockFileRepository.ValidateMediaFileDelete(It.IsAny<int>()))
                .Returns(fileDeleteErrors);

            mockFileRepository
                .Setup(mockFileRepository => mockFileRepository.GetMediaFileByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(file);

            _mockFileServicesClient.Setup(_mockServicesClient => _mockServicesClient.ExecuteAsync<FileDeleteResponse>(It.IsAny<RestRequest>())).ReturnsAsync(
            deleteResponse);
            _mockFileServicesClient.Setup(_mockFileServicesClient => _mockFileServicesClient.FileServicesUrl).Returns("http://localhost:5000");

            services.AddTransient(options => mockFileRepository.Object);
            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient(options => _mockFileServicesClient.Object);

            return services.BuildServiceProvider();
        }
    }
}
