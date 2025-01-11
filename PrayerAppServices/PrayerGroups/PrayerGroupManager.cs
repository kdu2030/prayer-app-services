using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Users;
using PrayerAppServices.Users.Models;
using PrayerAppServices.Utils;

namespace PrayerAppServices.PrayerGroups {
    public class PrayerGroupManager(IPrayerGroupRepository prayerGroupRepository, IUserManager userManager) : IPrayerGroupManager {
        private readonly IPrayerGroupRepository _prayerGroupRepository = prayerGroupRepository;
        private readonly IUserManager _userManager = userManager;

        public PrayerGroupDetails CreatePrayerGroup(string authToken, NewPrayerGroupRequest newPrayerGroupRequest) {
            string username = _userManager.ExtractUsernameFromAuthHeader(authToken);
            string? colorStr = newPrayerGroupRequest.Color;
            int? color = colorStr != null ? ColorUtils.ColorHexStringToInt(colorStr) : null;
            NewPrayerGroup newPrayerGroup = new NewPrayerGroup {
                GroupName = newPrayerGroupRequest.GroupName,
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
                GroupName = createResponse.GroupName,
                Description = createResponse.Description,
                Rules = createResponse.Rules,
                Color = colorStr,
                ImageFile = groupImage,
                Admins = adminUsers,
                IsUserJoined = true,
                UserRole = PrayerGroupRole.Admin,
            };

            return prayerGroupDetails;
        }

        public PrayerGroupDetails GetPrayerGroupDetails(string authHeader, int prayerGroupId) {
            string username = _userManager.ExtractUsernameFromAuthHeader(authHeader);

            PrayerGroup? prayerGroup = _prayerGroupRepository.GetPrayerGroupById(prayerGroupId);
            if (prayerGroup == null) {
                throw new ArgumentException($"A prayer group with id {prayerGroupId} does not exist");
            }

            IEnumerable<PrayerGroupAdminUser> adminUsers = _prayerGroupRepository.GetPrayerGroupAdmins(prayerGroupId);
            PrayerGroupAppUser? appUser = _prayerGroupRepository.GetPrayerGroupAppUser(prayerGroupId, username);

            IEnumerable<UserSummary> adminUserSummaries = GetAdminUserSummaries(adminUsers);
            string? colorString = prayerGroup.Color.HasValue ? ColorUtils.ColorIntToHexString(prayerGroup.Color ?? 0) : null;

            PrayerGroupDetails prayerGroupDetails = new PrayerGroupDetails {
                Id = prayerGroupId,
                GroupName = prayerGroup.GroupName,
                Description = prayerGroup.Description,
                Rules = prayerGroup.Rules,
                ImageFile = prayerGroup.ImageFile,
                Admins = adminUserSummaries,
                Color = colorString,
                IsUserJoined = appUser != null,
                UserRole = appUser?.PrayerGroupRole,
            };

            return prayerGroupDetails;
        }

        private static MediaFileBase? GetGroupImageFromCreateResponse(CreatePrayerGroupResponse response) {
            if (response.ImageFileId == null) {
                return null;
            }
            return new MediaFileBase {
                Id = response.ImageFileId,
                FileName = response.GroupImageFileName ?? "",
                Url = response.GroupImageFileUrl ?? "",
                FileType = FileType.Image,
            };
        }

        private static IEnumerable<UserSummary>? GetAdminUserFromCreateResponse(CreatePrayerGroupResponse response) {
            if (response.AdminUserId == null) {
                return null;
            }

            MediaFileBase? userImage = response.AdminImageFileId != null ?
                new MediaFileBase {
                    Id = response.AdminImageFileId,
                    FileName = response.AdminImageFileName ?? "",
                    Url = response.AdminImageFileUrl ?? "",
                    FileType = FileType.Image
                }
                : null;

            UserSummary adminUserSummary = new UserSummary {
                Id = response.AdminUserId ?? -1,
                FullName = response.AdminFullName,
                Image = userImage
            };

            return [adminUserSummary];
        }

        private IEnumerable<UserSummary> GetAdminUserSummaries(IEnumerable<PrayerGroupAdminUser> adminUsers) {
            return adminUsers.Where(adminUser => adminUser.Id != null)
                .Select(adminUser => new UserSummary {
                    Id = adminUser.Id ?? -1,
                    FullName = adminUser.FullName,
                    Image = adminUser.ImageFileId != null ? new MediaFileBase {
                        Id = adminUser.ImageFileId,
                        FileName = adminUser.FileName ?? "",
                        Url = adminUser.FileUrl ?? "",
                        FileType = FileType.Image
                    } : null
                });
        }

    }
}
