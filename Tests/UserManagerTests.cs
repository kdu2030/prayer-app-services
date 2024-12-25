using Microsoft.Extensions.DependencyInjection;
using PrayerAppServices.User;
using PrayerAppServices.User.Models;

namespace Tests {
    public class UserManagerTests {
        private ServiceProvider _serviceProvider;

        [SetUp]
        public void Setup() {
            IServiceCollection services = new ServiceCollection();
            services.AddTestServices();
            services.AddTransient<IUserManager, UserManager>();
            _serviceProvider = services.BuildServiceProvider();
        }

        [TearDown]
        public void TearDown() {
            _serviceProvider.TearDownTestDb();
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