using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PrayerAppServices.Configuration {
    public static class JwtConfiguration {
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration) {
            string? jwtKey = configuration["Jwt:Key"];
            if (jwtKey == null) {
                throw new ArgumentNullException(nameof(jwtKey));
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            services.AddSingleton<JwtSecurityTokenHandler>();

        }
    }
}
