namespace PrayerAppServices.PrayerGroups.DTOs
{
    public class PrayerGroupQuery
    {
        public required int TargetPrayerGroupId { get; set; }
        public required int TargetUserId { get; set; }
    }
}
