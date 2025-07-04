﻿using PrayerAppServices.Common.Sorting;


namespace PrayerAppServices.PrayerRequests.Models {
    public class PrayerRequestFilterCriteria {
        public IEnumerable<int>? PrayerGroupIds { get; set; }
        public IEnumerable<int>? CreatorUserIds { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public int? BookmarkedByUserId { get; set; }
        public required SortConfig SortConfig { get; set; }
        public bool IncludeExpiredRequests { get; set; } = false;
    }
}
