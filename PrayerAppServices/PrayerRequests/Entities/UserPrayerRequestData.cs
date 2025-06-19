namespace PrayerAppServices.PrayerRequests.Entities {
    public class UserPrayerRequestData {
        public IEnumerable<int?>? UserLikedRequestIds { get; set; }
        public IEnumerable<int?>? UserPrayedRequestIds { get; set; }
        public IEnumerable<int?>? UserCommentedPrayerRequestIds { get; set; }
    }
}
