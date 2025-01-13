using PrayerAppServices.Files.Entities;
using System.ComponentModel.DataAnnotations;

namespace PrayerAppServices.Users.Models {
    public class UserSummary {
        public int Id { get; set; }
        public string? Username { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? FullName { get; set; }

        public UserTokenPair? Tokens { get; set; }

        public MediaFileBase? Image { get; set; }

    }
}
