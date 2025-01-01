using Microsoft.Extensions.DependencyInjection;
using Moq;
using PrayerAppServices.Files;
using PrayerAppServices.Files.Models;
using RestSharp;

namespace Tests {
    public class FileManagerTests {
        private ServiceProvider _serviceProvider;
        private Mock<IRestClient> _mockRestClient;

        [SetUp]
        public void SetUp() {
            IServiceCollection services = new ServiceCollection();
            services.AddTestServices();
            services.AddTransient<IFileManager, FileManager>();
            services.AddTransient(options => _mockRestClient.Object);
            _serviceProvider = services.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown() {
            _serviceProvider.TearDownTestDb();
            _serviceProvider.Dispose();
        }

        [Test]
        public void UploadFileAsync_WhenGivenValidFile_ReturnsMediaFile() {
            RestRequest mockRequest = new RestRequest("/file", Method.Post);

            RestResponse<FileUploadResponse> mockResponse = new RestResponse<FileUploadResponse>(mockRequest);
            mockResponse.Data = new FileUploadResponse { IsError = false, Url = "" };

            _mockRestClient.Setup(_mockRestClient =>
                _mockRestClient.ExecuteAsync<FileUploadResponse>(It.IsAny<RestRequest>(), new CancellationToken()))
            .ReturnsAsync(() => mockResponse);

        }
    }
}
