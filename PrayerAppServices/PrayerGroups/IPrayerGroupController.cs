﻿using Microsoft.AspNetCore.Mvc;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public interface IPrayerGroupController {
        Task<ActionResult<PrayerGroupDetails>> CreatePrayerGroupAsync(string authHeader, PrayerGroupRequest newPrayerGroupRequest);
        Task<ActionResult<PrayerGroupDetails>> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId);
        Task<ActionResult<GroupNameValidationResponse>> ValidateGroupNameAsync(string prayerGroupName);
        ActionResult<IEnumerable<PrayerGroupDetails>> SearchByGroupName(string nameQuery, int maxResults);
        Task<ActionResult<PrayerGroupDetails>> UpdatePrayerGroupAsync(int prayerGroupId, PrayerGroupRequest prayerGroupRequest);
        Task<ActionResult<PrayerGroupUsersResponse>> GetPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupRole>? roles);
        Task<ActionResult> UpdatePrayerGroupAdminsAsync(string authHeader, int prayerGroupId, UpdatePrayerGroupAdminsRequest updateAdminsRequest);
        Task<ActionResult> AddPrayerGroupUsersAsync(int prayerGroupId, AddPrayerGroupUserRequest request);
        Task<ActionResult> DeletePrayerGroupUsersAsync(string authHeader, int prayerGroupId, PrayerGroupDeleteRequest request);
    }
}
