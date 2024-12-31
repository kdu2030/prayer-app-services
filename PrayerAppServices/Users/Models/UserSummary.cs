using System.ComponentModel.DataAnnotations;

namespace PrayerAppServices.Users.Models {
    public class UserSummary {
        public int Id { get; set; }
        public required string Username { get; set; }

        [EmailAddress]
        public required string Email { get; set; }

        public required string FullName { get; set; }

        public required UserTokenPair Tokens { get; set; }

    }
}
