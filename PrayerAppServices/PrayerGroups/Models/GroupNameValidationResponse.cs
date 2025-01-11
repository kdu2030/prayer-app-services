namespace PrayerAppServices.PrayerGroups.Models {
    public class GroupNameValidationResponse {
        public required bool IsNameValid { get; set; }
        public required IEnumerable<string> Errors { get; set; }
    }
}
