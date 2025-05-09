﻿using PrayerAppServices.Files;
using PrayerAppServices.PrayerGroups;
using PrayerAppServices.PrayerRequests;
using PrayerAppServices.Users;

namespace PrayerAppServices.Configuration {
    public static class ServiceConfiguration {

        public static void RegisterServices(this IServiceCollection services) {
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IMediaFileRepository, MediaFileRepository>();
            services.AddSingleton<IFileServicesClient, FileServicesClient>();
            services.AddScoped<IPrayerGroupRepository, PrayerGroupRepository>();
            services.AddScoped<IPrayerGroupManager, PrayerGroupManager>();
            services.AddScoped<IPrayerRequestRepository, PrayerRequestRepository>();
            services.AddScoped<IPrayerRequestManager, PrayerRequestManager>();

        }

    }
}
