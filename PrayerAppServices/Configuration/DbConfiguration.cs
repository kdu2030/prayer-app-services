using Microsoft.EntityFrameworkCore;
using Npgsql;
using PrayerAppServices.Data;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;
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

            services.AddSingleton(sp => {
                NpgsqlDataSourceBuilder builder = new NpgsqlDataSourceBuilder(connectionString);
                builder.MapEnum<PrayerGroupRole>("prayer_group_role");
                builder.MapComposite<PrayerGroupUserToAdd>("prayer_group_user_to_add");
                return builder.Build();
            });
        }
    }
}
