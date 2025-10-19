namespace PrayerAppServices.PrayerGroups.Constants
{
    public static class PrayerGroupValidationErrors
    {
        public const string MustBeAdminToModifyPrayerGroup = "User must be an admin to modify prayer group.";
        public const string CannotBePublicWithActiveJoinRequests = "A prayer group cannot be public with active join requests.";
        public const string CannotUseNonImageForAvatar = "Cannot use a non-image as a prayer group image";
        public const string CannotUseNonImageForBanner = "Cannot use a non-image as a prayer group banner image.";
        public const string UserSortFieldNotSupported = "Prayer group user sort field not supported.";
    }
}
