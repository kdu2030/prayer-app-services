using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Frameworks;
using PrayerAppServices.Data;
using PrayerAppServices.User;
using PrayerAppServices.User.Entities;
using PrayerAppServices.User.Models;
using System.ComponentModel.DataAnnotations;

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
        public async Task CreateUserAsync_GivenUniqueValues_ReturnsUserSummary() {
            using IServiceScope scope = _serviceProvider.CreateScope();
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

        [Test]
        public async Task CreateUserAsync_GivenDuplicateUsername_ThrowsException() {
            using IServiceScope scope = _serviceProvider.CreateScope();
            UserManager<AppUser>? aspUserManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
            IUserManager? userManager = scope.ServiceProvider.GetService<IUserManager>();

            if (aspUserManager == null || userManager == null) {
                Assert.Fail("User manager or database context does not exist.");
            }

            await aspUserManager!.CreateAsync(new AppUser {
                UserName = "dshrute",
                FullName = "Dwight Schrute",
                Email = "dshrute@example.com",
                PasswordHash = "beets",
            });

            CreateUserRequest request = new CreateUserRequest {
                Username = "dshrute",
                FullName = "Dwight Schrute",
                Email = "dshrute@dundermifflin.com",
                Password = "beets"
            };

            Assert.ThrowsAsync<ArgumentException>(() => userManager!.CreateUserAsync(request));
        }
    }
}