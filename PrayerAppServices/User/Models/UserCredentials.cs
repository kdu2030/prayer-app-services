using System.ComponentModel.DataAnnotations;

namespace PrayerAppServices.User.Models {
    public class UserCredentials {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
