namespace PrayerAppServices.Error {
    public class ValidationErrorException : Exception {
        public IEnumerable<string> ValidationErrors { get; set; }
        public ValidationErrorException(IEnumerable<string> validationErrors) : base(FormatExceptionMessage(validationErrors)) {
            ValidationErrors = validationErrors;
        }

        private static string FormatExceptionMessage(IEnumerable<string> validationErrors) {
            return $"[{string.Join(", ", validationErrors)}]";
        }
    }
}
