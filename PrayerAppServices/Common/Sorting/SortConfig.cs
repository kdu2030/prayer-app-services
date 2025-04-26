namespace PrayerAppServices.Common.Sorting {
    public class SortConfig<T> {
        public required T SortField { get; set; }
        public required SortOrder SortOrder { get; set; }
    }
}
