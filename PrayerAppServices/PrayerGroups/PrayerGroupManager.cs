using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Users;
using PrayerAppServices.Users.Models;
using PrayerAppServices.Utils;

namespace PrayerAppServices.PrayerGroups {
    public class PrayerGroupManager(IPrayerGroupRepository prayerGroupRepository, IUserManager userManager) : IPrayerGroupManager {
        private readonly IPrayerGroupRepository _prayerGroupRepository = prayerGroupRepository;
        private readonly IUserManager _userManager = userManager;

        // TODO: Instead of IsUserAdmin, create a new field called User Role
        public PrayerGroupDetails CreatePrayerGroup(string authToken, NewPrayerGroupRequest newPrayerGroupRequest) {
            string username = _userManager.ExtractUsernameFromAuthHeader(authToken);
            string? colorStr = newPrayerGroupRequest.Color;
            int? color = colorStr != null ? ColorUtils.ColorHexStringToInt(colorStr) : null;
            NewPrayerGroup newPrayerGroup = new NewPrayerGroup {
                Name = newPrayerGroupRequest.Name,
                Description = newPrayerGroupRequest.Description,
                Rules = newPrayerGroupRequest.Rules,
                Color = color,
                ImageFileId = newPrayerGroupRequest.ImageFileId
            };

            CreatePrayerGroupResponse createResponse = _prayerGroupRepository.CreatePrayerGroup(username, newPrayerGroup); ;
            MediaFileBase? groupImage = GetGroupImageFromCreateResponse(createResponse);
            IEnumerable<UserSummary>? adminUsers = GetAdminUserFromCreateResponse(createResponse);

            PrayerGroupDetails prayerGroupDetails = new PrayerGroupDetails {
                Id = createResponse.Id,
                Name = createResponse.Name,
                Description = createResponse.Description,
                Rules = createResponse.Rules,
                Color = colorStr,
                ImageFile = groupImage,
                Admins = adminUsers,
                IsUserJoined = true,
                IsUserAdmin = true,
            };

            return prayerGroupDetails;
        }

        private static MediaFileBase? GetGroupImageFromCreateResponse(CreatePrayerGroupResponse response) {
            if (response.ImageFileId == null) {
                return null;
            }
            return new MediaFileBase {
                Id = response.ImageFileId,
                Name = response.GroupImageFileName ?? "",
                Url = response.GroupImageFileUrl ?? "",
                Type = FileType.Image,
            };
        }

        private static IEnumerable<UserSummary>? GetAdminUserFromCreateResponse(CreatePrayerGroupResponse response) {
            if (response.AdminUserId == null) {
                return null;
            }

            MediaFileBase? userImage = response.ImageFileId != null ?
                new MediaFileBase {
                    Id = response.AdminImageFileId,
                    Name = response.AdminImageFileName ?? "",
                    Url = response.AdminImageFileUrl ?? "",
                    Type = FileType.Image
                }
                : null;

            UserSummary adminUserSummary = new UserSummary {
                Id = response.AdminUserId ?? -1,
                FullName = response.AdminFullName,
                Image = userImage
            };

            return [adminUserSummary];
        }
    }
}
