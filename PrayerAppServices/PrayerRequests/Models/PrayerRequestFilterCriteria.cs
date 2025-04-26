using PrayerAppServices.Common.Sorting;


namespace PrayerAppServices.PrayerRequests.Models {
    public class PrayerRequestFilterCriteria {
        public int[]? PrayerGroupIds { get; set; }
        public int[]? CreatorUserIds { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public required SortConfig<string> SortConfig { get; set; }
    }
}
