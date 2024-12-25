using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PrayerAppServices.Data;
using PrayerAppServices.User;
using PrayerAppServices.User.Entities;
using PrayerAppServices.User.Models;

namespace Tests {
    public class UserManagerTests {
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup() {
            // TODO: Need to add an IConfiguration to the service collection
            // Use in memory configuration
            IServiceCollection services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));
            services.AddTransient<IUserManager, UserManager>();
            services.AddIdentityApiEndpoints<AppUser>()
                .AddEntityFrameworkStores<AppDbContext>();
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
        public async Task CreateUser_GivenUniqueValues_ReturnsUserSummary() {
            using var scope = _serviceProvider.CreateScope();
            IUserManager? userManager = scope.ServiceProvider.GetService<IUserManager>();

            if (userManager == null) {
                Assert.Fail("User manager does not exist.");
            }

            CreateUserRequest request = new CreateUserRequest {
                Username = "dshrute",
                FullName = "Dwight Schrute",
                Email = "dshrute@dundermifflin.com",
                Password = "beets",
            };

            UserSummary userSummary = await userManager!.CreateUserAsync(request);

            Assert.Multiple(() => {
                Assert.That(userSummary.Username, Is.EqualTo(request.Username));
                Assert.That(userSummary.Email, Is.EqualTo(request.Email));
            });
        }
    }
}