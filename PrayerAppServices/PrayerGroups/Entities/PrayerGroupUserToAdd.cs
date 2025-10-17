namespace PrayerAppServices.PrayerGroups.Entities
{
    public class PrayerGroupUserToAdd
    {
        public required int UserId { get; set; }
        public required int PrayerGroupRole { get; set; }
    }
}
