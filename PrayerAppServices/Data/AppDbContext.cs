using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PrayerAppServices.User.Entities;

namespace PrayerAppServices.Data {
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, IdentityRole<int>, int>(options) {
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<IdentityUser<int>>().ToTable("asp_net_users");
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("asp_net_user_tokens").HasNoKey();
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("asp_net_user_logins").HasNoKey();
            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("asp_net_user_claims");
            modelBuilder.Entity<IdentityRole>().ToTable("asp_net_roles");
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("asp_net_user_roles").HasNoKey();
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("asp_net_role_claims");
        }

    }
}
