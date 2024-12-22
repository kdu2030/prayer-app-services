namespace PrayerAppServices.User.Models {
    public class UserTokenPair {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
