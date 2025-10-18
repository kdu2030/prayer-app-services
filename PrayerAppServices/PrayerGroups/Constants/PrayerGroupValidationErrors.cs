namespace PrayerAppServices.PrayerGroups.Constants
{
    public static class PrayerGroupValidationErrors
    {
        public static readonly string MustBeAdminToModifyPrayerGroup = "User must be an admin to modify prayer group.";
        public static readonly string CannotBePublicWithActiveJoinRequests = "A prayer group cannot be public with active join requests.";
        public static readonly string CannotUseNonImageForAvatar = "Cannot use a non-image as a prayer group image";
        public static readonly string CannotUseNonImageForBanner = "Cannot use a non-image as a prayer group banner image.";
    }
}
