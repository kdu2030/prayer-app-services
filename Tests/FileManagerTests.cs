using Microsoft.Extensions.DependencyInjection;
using PrayerAppServices.Files;

namespace Tests {
    public class FileManagerTests {
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void SetUp() {
            IServiceCollection services = new ServiceCollection();
            services.AddTestServices();
            services.AddTransient<IFileManager, FileManager>();
            _serviceProvider = services.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown() {
            _serviceProvider.TearDownTestDb();
            _serviceProvider.Dispose();
        }

        [Test]

    }
}
