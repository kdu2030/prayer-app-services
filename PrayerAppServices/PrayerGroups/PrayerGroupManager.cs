using Microsoft.EntityFrameworkCore;
using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.DTOs;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Users;
using PrayerAppServices.Users.Models;
using PrayerAppServices.Utils;

namespace PrayerAppServices.PrayerGroups {
    public class PrayerGroupManager(IPrayerGroupRepository prayerGroupRepository, IUserManager userManager) : IPrayerGroupManager {
        private readonly IPrayerGroupRepository _prayerGroupRepository = prayerGroupRepository;
        private readonly IUserManager _userManager = userManager;

        public async Task<PrayerGroupDetails> CreatePrayerGroupAsync(string authToken, PrayerGroupRequest newPrayerGroupRequest) {
            string username = _userManager.ExtractUsernameFromAuthHeader(authToken);
            string? colorStr = newPrayerGroupRequest.Color;
            int? color = colorStr != null ? ColorUtils.ColorHexStringToInt(colorStr) : null;
            PrayerGroupDTO newPrayerGroup = new PrayerGroupDTO {
                GroupName = newPrayerGroupRequest.GroupName,
                Description = newPrayerGroupRequest.Description,
                Rules = newPrayerGroupRequest.Rules,
                Color = color,
                ImageFileId = newPrayerGroupRequest.ImageFileId
            };

            PrayerGroupDetailsEntity createResponse = await _prayerGroupRepository.CreatePrayerGroupAsync(username, newPrayerGroup); ;
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

        public async Task<PrayerGroupDetails> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId) {
            string username = _userManager.ExtractUsernameFromAuthHeader(authHeader);

            Task<PrayerGroup?> prayerGroupTask = _prayerGroupRepository.GetPrayerGroupByIdAsync(prayerGroupId, true);
            Task<IEnumerable<PrayerGroupUserEntity>> adminUsersTask = _prayerGroupRepository.GetPrayerGroupUsersAsync(prayerGroupId, [PrayerGroupRole.Admin]);
            Task<PrayerGroupAppUser?> appUserTask = _prayerGroupRepository.GetPrayerGroupAppUserAsync(prayerGroupId, username);

            PrayerGroup? prayerGroup = await prayerGroupTask;
            IEnumerable<PrayerGroupUserEntity> adminUsers = await adminUsersTask;
            PrayerGroupAppUser? appUser = await appUserTask;

            if (prayerGroup == null) {
                throw new ArgumentException($"A prayer group with id {prayerGroupId} does not exist");
            }

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

        public GroupNameValidationResponse ValidateGroupName(string groupName) {
            List<string> errors = new List<string>();
            PrayerGroup? prayerGroup = _prayerGroupRepository.GetPrayerGroupByName(groupName);
            if (prayerGroup != null) {
                errors.Add("A prayer group with this name already exists.");
            }

            return new GroupNameValidationResponse { IsNameValid = errors.Count == 0, Errors = errors };
        }

        public IEnumerable<PrayerGroupDetails> SearchPrayerGroupsByName(string nameQuery, int maxNumResults) {
            IEnumerable<PrayerGroupSearchResult> searchResults = _prayerGroupRepository.SearchPrayerGroupsByName(nameQuery, maxNumResults);
            return searchResults.Select(GetPrayerGroupDetailFromSearchResult);
        }

        public async Task<PrayerGroup> UpdatePrayerGroupAsync(int prayerGroupId, PrayerGroupDTO prayerGroupDTO) {
            PrayerGroup? prayerGroup = await _prayerGroupRepository.GetPrayerGroupByIdAsync(prayerGroupId);
            if (prayerGroup == null) {
                throw new KeyNotFoundException($"A prayer group with id {prayerGroupId} does not exist");
            }

            int? imageFileId = prayerGroupDTO.ImageFileId;
            //MediaFile? mediaFile = imageFileId.HasValue ? _dbContext.MediaFiles.First(mediaFile => mediaFile.Id == imageFileId) : null;
            throw new NotImplementedException();
        }

        private PrayerGroupDetails GetPrayerGroupDetailFromSearchResult(PrayerGroupSearchResult searchResult) {
            MediaFileBase? mediaFile = searchResult.ImageFileId != null
                ? new MediaFileBase {
                    Id = searchResult.ImageFileId,
                    FileName = searchResult.FileName ?? "",
                    FileType = searchResult.FileType ?? FileType.Unknown,
                    Url = searchResult.FileUrl ?? ""
                }
                : null;
            return new PrayerGroupDetails {
                Id = searchResult.Id,
                GroupName = searchResult.GroupName,
                ImageFile = mediaFile,
            };
        }

        private static MediaFileBase? GetGroupImageFromCreateResponse(PrayerGroupDetailsEntity response) {
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

        private static IEnumerable<UserSummary>? GetAdminUserFromCreateResponse(PrayerGroupDetailsEntity response) {
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

        private IEnumerable<UserSummary> GetAdminUserSummaries(IEnumerable<PrayerGroupUserEntity> adminUsers) {
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
