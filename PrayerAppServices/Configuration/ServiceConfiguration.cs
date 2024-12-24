﻿using PrayerAppServices.User;

namespace PrayerAppServices.Configuration {
    public static class ServiceConfiguration {

        public static void RegisterServices(this IServiceCollection services) {
            services.AddScoped<IUserManager, UserManager>();
        }

    }
}