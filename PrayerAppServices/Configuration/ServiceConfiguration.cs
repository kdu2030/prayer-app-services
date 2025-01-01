using PrayerAppServices.Files;
using PrayerAppServices.Users;
using RestSharp;

namespace PrayerAppServices.Configuration {
    public static class ServiceConfiguration {

        public static void RegisterServices(this IServiceCollection services) {
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IMediaFileRepository, MediaFileRepository>();
            services.AddSingleton<IFileServicesClient, FileServicesClient>();
        }

    }
}
