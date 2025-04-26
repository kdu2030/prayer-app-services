namespace PrayerAppServices.PrayerRequests.Constants {
    public static class PrayerRequestSortFields {
        public static readonly string CreatedAt = "CreatedDate";
        public static readonly string LikeCount = "LikeCount";
        public static readonly string CommentCount = "CommentCount";
        public static readonly string PrayedCount = "PrayedCount";

        public static readonly string[] ValidSortFields = {
            CreatedAt,
            LikeCount,
            CommentCount,
            PrayedCount
        };
    }
}
