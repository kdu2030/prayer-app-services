using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
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
