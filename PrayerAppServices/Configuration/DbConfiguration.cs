﻿using Microsoft.EntityFrameworkCore;
using PrayerAppServices.Data;
using PrayerAppServices.Users.Entities;

namespace PrayerAppServices.Configuration {
    public static class DbConfiguration {
        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration) {
            string? connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString)) {
                throw new ArgumentNullException(nameof(connectionString));
            }

            services.AddDbContext<AppDbContext>(options =>
              options.UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
            );

            services.AddIdentityApiEndpoints<AppUser>()
                .AddEntityFrameworkStores<AppDbContext>();
        }
    }
}
