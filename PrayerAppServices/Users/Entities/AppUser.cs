using Microsoft.AspNetCore.Identity;
using PrayerAppServices.Files.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrayerAppServices.Users.Entities {
    public class AppUser : IdentityUser<int> {
        [Column(TypeName = "varchar(256)")]
        public string? FullName { get; set; }

        public MediaFile? ImageFile { get; set; }

        public AppUser() : base() {
        }

        public AppUser(string username, string fullName, string email, string passwordHash) : base(username) {
            FullName = fullName;
            Email = email;
            PasswordHash = passwordHash;
        }

    }
}
