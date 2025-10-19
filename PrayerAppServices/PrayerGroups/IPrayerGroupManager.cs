using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups
{
    public interface IPrayerGroupManager
    {
        Task<PrayerGroupModel> CreatePrayerGroupAsync(string authToken, PrayerGroupRequest newPrayerGroupRequest);
        Task<PrayerGroupModel> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId);
        Task<GroupNameValidationResponse> ValidateGroupNameAsync(string groupName);
        Task<IEnumerable<PrayerGroupModel>> SearchPrayerGroupsAsync(PrayerGroupSearchRequest prayerGroupSearchRequest);
        Task<PrayerGroupModel> UpdatePrayerGroupAsync(string authHeader, int prayerGroupId, PrayerGroupRequest prayerGroupRequest);
        Task<PrayerGroupUsersResponse> GetPrayerGroupUsersAsync(int prayerGroupId, PrayerGroupUsersGetRequest usersGetRequest);
        Task UpdatePrayerGroupAdminsAsync(string authHeader, int prayerGroupId, UpdatePrayerGroupAdminsRequest updateAdminsRequest);
        Task AddPrayerGroupUsersAsync(int prayerGroupId, AddPrayerGroupUserRequest request);
        Task DeletePrayerGroupUsersAsync(string authHeader, int prayerGroupId, PrayerGroupDeleteRequest request);
        Task<bool> IsPrayerGroupAdminAsync(string authHeader, int prayerGroupId);
    }
}
