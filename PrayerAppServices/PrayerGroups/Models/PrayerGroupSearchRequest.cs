namespace PrayerAppServices.PrayerGroups.Models
{
    public class PrayerGroupSearchRequest
    {
        public required string GroupNameQuery { get; set; }
        public int? MaxNumResults { get; set; }
    }
}
