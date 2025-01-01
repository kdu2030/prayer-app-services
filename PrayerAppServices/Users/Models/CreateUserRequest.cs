using System.ComponentModel.DataAnnotations;

namespace PrayerAppServices.Users.Models {
    public class CreateUserRequest {
        public required string Username { get; set; }
        public required string FullName { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
