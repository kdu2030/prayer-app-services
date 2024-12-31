using PrayerAppServices.Files;
using PrayerAppServices.Users;

namespace PrayerAppServices.Configuration {
    public static class ServiceConfiguration {

        public static void RegisterServices(this IServiceCollection services) {
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IFileManager, FileManager>();
        }

    }
}
