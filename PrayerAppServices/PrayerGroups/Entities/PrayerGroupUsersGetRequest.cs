using PrayerAppServices.Common.Sorting;
using PrayerAppServices.PrayerGroups.Constants;

namespace PrayerAppServices.PrayerGroups.Entities
{
    public class PrayerGroupUsersGetRequest
    {
        public IEnumerable<PrayerGroupRole>? PrayerGroupRoles { get; set; }
        public SortConfig? SortConfig { get; set; }
    }
}
