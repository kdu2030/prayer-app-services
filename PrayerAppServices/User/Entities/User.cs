using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrayerAppServices.User.Entities {
    [Table("app_user")]
    public class User : IdentityUser<int> {
        public required string FullName { get; set; }

        public User(string username, string fullName, string email, string passwordHash) : base(username) {
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
        }

    }
}
