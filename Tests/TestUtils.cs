

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PrayerAppServices.Configuration;
using PrayerAppServices.Data;
using PrayerAppServices.Users.Entities;

namespace Tests {
    public static class TestUtils {
        private static readonly Dictionary<string, string> JwtConfiguration = new Dictionary<
           string,
           string
       >
       {
            { "Jwt:Key", "459b5631306a7ec4573e52e83bcc1a22ce2e19682f5e31bb965a9709f1fd99c1" },
            { "Jwt:Issuer", "http://localhost:5152/" },
            { "Jwt:Audience", "http://localhost:5152/" },
        };

        public static void AddTestServices(this IServiceCollection services) {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(JwtConfiguration)
                .Build();

            services.AddSingleton(configuration);
            services.AddJwtConfiguration(configuration);
            services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDatabase"));
            services.AddIdentityApiEndpoints<AppUser>()
                .AddEntityFrameworkStores<AppDbContext>();
        }

        public static void TearDownTestDb(this ServiceProvider serviceProvider) {
            using var scope = serviceProvider.CreateScope();
            AppDbContext? context = scope.ServiceProvider.GetService<AppDbContext>();
            context?.Database.EnsureDeleted();
        }
    }
}
