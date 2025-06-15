namespace PrayerAppServices.PrayerRequests.Constants {
    public static class PrayerRequestSortFields {
        public const string CreatedAt = "CreatedDate";
        public const string LikeCount = "LikeCount";
        public const string CommentCount = "CommentCount";
        public const string PrayedCount = "PrayedCount";

        public static readonly string[] ValidSortFields = {
            CreatedAt,
            LikeCount,
            CommentCount,
            PrayedCount
        };
    }
}
