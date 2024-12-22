namespace PrayerAppServices.Error {
    public class Error {
        public required string ErrorCode { get; set; }
        public required string Message { get; set; }
        public required string Url { get; set; }
        public required string RequestMethod { get; set; }
    }
}
