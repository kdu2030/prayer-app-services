using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrayerAppServices.Data;
using PrayerAppServices.User;

namespace Tests {
    public class UserManagerTests {
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup() {
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));
            services.AddTransient<IUserManager, UserManager>();
            _serviceProvider = services.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown() {
            using var scope = _serviceProvider.CreateScope();
            AppDbContext? context = scope.ServiceProvider.GetService<AppDbContext>();
            context?.Database.EnsureDeleted();
            _serviceProvider.Dispose();
        }

        [Test]
        public void Test1() {
            Assert.Pass();
        }
    }
}