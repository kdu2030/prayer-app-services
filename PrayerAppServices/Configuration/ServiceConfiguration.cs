using PrayerAppServices.Files;
using PrayerAppServices.Users;
using RestSharp;

namespace PrayerAppServices.Configuration {
    public static class ServiceConfiguration {

        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration) {
            string fileServicesUrl = configuration["FileUpload:Url"]
                ?? throw new NullReferenceException("File Upload URL cannot be null.");

            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IMediaFileRepository, MediaFileRepository>();
            services.AddSingleton<IRestClient, RestClient>(options => new RestClient(fileServicesUrl));
        }

    }
}
