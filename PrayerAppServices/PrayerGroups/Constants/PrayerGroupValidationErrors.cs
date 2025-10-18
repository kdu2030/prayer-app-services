namespace PrayerAppServices.PrayerGroups.Constants
{
    public static class PrayerGroupValidationErrors
    {
        public static readonly string MustBeAdminToModifyPrayerGroup = "User must be an admin to modify prayer group.";
        public static readonly string CannotBePublicWithActiveJoinRequests = "A prayer group cannot be public with active join requests.";
    }
}
