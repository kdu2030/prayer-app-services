namespace PrayerAppServices.Error {
    public class DataValidationError : Error {
        public required IEnumerable<string> DataValidationErrors { get; set; }
    }
}
