﻿using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupManager {
        Task<PrayerGroupDetails> CreatePrayerGroupAsync(string authToken, PrayerGroupRequest newPrayerGroupRequest);
        Task<PrayerGroupDetails> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId);
        Task<GroupNameValidationResponse> ValidateGroupNameAsync(string groupName);
        IEnumerable<PrayerGroupDetails> SearchPrayerGroupsByName(string nameQuery, int maxNumResults);
        Task<PrayerGroupDetails> UpdatePrayerGroupAsync(int prayerGroupId, PrayerGroupRequest prayerGroupRequest);
        Task<PrayerGroupUsersResponse> GetPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupRole>? prayerGroupRoles);
        Task UpdatePrayerGroupAdminsAsync(string authHeader, int prayerGroupId, UpdatePrayerGroupAdminsRequest updateAdminsRequest);
        Task AddPrayerGroupUsersAsync(int prayerGroupId, AddPrayerGroupUserRequest request);
        Task DeletePrayerGroupUsersAsync(string authHeader, int prayerGroupId, PrayerGroupDeleteRequest request);
        Task<bool> IsPrayerGroupAdminAsync(string authHeader, int prayerGroupId);
    }
}
