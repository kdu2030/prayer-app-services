using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrayerAppServices.User.Entities {
    [Table("app_user")]
    public class AppUser : IdentityUser<int> {
        public string? FullName { get; set; }

        public AppUser() { }

        public AppUser(string username, string fullName, string email, string passwordHash) : base(username) {
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
        }

    }
}
