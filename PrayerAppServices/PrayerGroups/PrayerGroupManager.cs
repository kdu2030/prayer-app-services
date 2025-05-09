﻿using AutoMapper;
using PrayerAppServices.Files;
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
    public class PrayerGroupManager(IPrayerGroupRepository prayerGroupRepository, IUserManager userManager, IMediaFileRepository mediaFileRepository, IMapper mapper) : IPrayerGroupManager {
        private readonly IPrayerGroupRepository _prayerGroupRepository = prayerGroupRepository;
        private readonly IUserManager _userManager = userManager;
        private readonly IMediaFileRepository _mediaFileRepository = mediaFileRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<PrayerGroupDetails> CreatePrayerGroupAsync(string authToken, PrayerGroupRequest newPrayerGroupRequest) {
            string username = _userManager.ExtractUsernameFromAuthHeader(authToken);
            string? colorStr = newPrayerGroupRequest.Color;
            int? color = colorStr != null ? ColorUtils.ColorHexStringToInt(colorStr) : null;
            PrayerGroupDTO newPrayerGroup = new PrayerGroupDTO {
                GroupName = newPrayerGroupRequest.GroupName,
                Description = newPrayerGroupRequest.Description,
                Rules = newPrayerGroupRequest.Rules,
                Color = color,
                ImageFileId = newPrayerGroupRequest.ImageFileId,
                BannerImageFileId = newPrayerGroupRequest.BannerImageFileId,
            };

            PrayerGroupDetailsEntity createResponse = await _prayerGroupRepository.CreatePrayerGroupAsync(username, newPrayerGroup);

            MediaFileBase? groupImage = GetGroupImageFromCreateResponse(createResponse);
            MediaFileBase? bannerImage = GetGroupBannerImageFromCreateResponse(createResponse);

            IEnumerable<UserSummary>? adminUsers = GetAdminUserFromCreateResponse(createResponse);

            PrayerGroupDetails prayerGroupDetails = new PrayerGroupDetails {
                Id = createResponse.Id,
                GroupName = createResponse.GroupName,
                Description = createResponse.Description,
                Rules = createResponse.Rules,
                Color = colorStr,
                ImageFile = groupImage,
                BannerImageFile = bannerImage,
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
            Task<PrayerGroupAppUser?> appUserTask = _prayerGroupRepository.GetPrayerGroupAppUserByUsernameAsync(prayerGroupId, username);

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
                BannerImageFile = prayerGroup.BannerImageFile,
                Admins = adminUserSummaries,
                Color = colorString,
                IsUserJoined = appUser != null,
                UserRole = appUser?.PrayerGroupRole,
            };

            return prayerGroupDetails;
        }

        public async Task<GroupNameValidationResponse> ValidateGroupNameAsync(string groupName) {
            List<string> errors = new List<string>();
            PrayerGroup? prayerGroup = await _prayerGroupRepository.GetPrayerGroupByNameAsync(groupName);
            if (prayerGroup != null) {
                errors.Add("A prayer group with this name already exists.");
            }

            return new GroupNameValidationResponse { IsNameValid = errors.Count == 0, Errors = errors };
        }

        public IEnumerable<PrayerGroupDetails> SearchPrayerGroupsByName(string nameQuery, int maxNumResults) {
            IEnumerable<PrayerGroupSearchResult> searchResults = _prayerGroupRepository.SearchPrayerGroupsByName(nameQuery, maxNumResults);
            return searchResults.Select(GetPrayerGroupDetailFromSearchResult);
        }

        public async Task<PrayerGroupDetails> UpdatePrayerGroupAsync(int prayerGroupId, PrayerGroupRequest prayerGroupRequest) {
            PrayerGroup? existingPrayerGroup = await _prayerGroupRepository.GetPrayerGroupByNameAsync(prayerGroupRequest.GroupName, false);

            if (existingPrayerGroup != null && existingPrayerGroup?.Id != prayerGroupId) {
                throw new ArgumentException("A prayer group with this name already exists.");
            }

            int? imageFileId = prayerGroupRequest.ImageFileId;
            int? bannerImageFileId = prayerGroupRequest.BannerImageFileId;


            MediaFile? groupImageFile = await GetMediaFileByNullableIdAsync(imageFileId);
            MediaFile? bannerImageFile = await GetMediaFileByNullableIdAsync(bannerImageFileId);

            if (groupImageFile != null && groupImageFile.FileType != FileType.Image) {
                throw new ArgumentException("Cannot use a non-image as a prayer group image");
            }

            if (bannerImageFile != null && bannerImageFile.FileType != FileType.Image) {
                throw new ArgumentException("Cannot use a non-image as a prayer group banner image.");
            }

            PrayerGroup updatedPrayerGroup = _mapper.Map<PrayerGroup>(prayerGroupRequest, opts => {
                opts.Items["Id"] = prayerGroupId;
                opts.Items["ImageFile"] = groupImageFile;
                opts.Items["BannerImageFile"] = bannerImageFile;
            });

            await _prayerGroupRepository.UpdatePrayerGroupAsync(updatedPrayerGroup);
            return _mapper.Map<PrayerGroupDetails>(updatedPrayerGroup);
        }

        public async Task<PrayerGroupUsersResponse> GetPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupRole>? prayerGroupRoles) {
            IEnumerable<PrayerGroupRole> rolesToSearch = prayerGroupRoles == null || prayerGroupRoles.Count() == 0 ? [PrayerGroupRole.Member, PrayerGroupRole.Admin] : prayerGroupRoles;
            IEnumerable<PrayerGroupUserEntity> prayerGroupUsers = await _prayerGroupRepository.GetPrayerGroupUsersAsync(prayerGroupId, rolesToSearch);
            IEnumerable<PrayerGroupUserSummary> prayerGroupUserSummaries = _mapper.Map<IEnumerable<PrayerGroupUserSummary>>(prayerGroupUsers);
            return new PrayerGroupUsersResponse { Users = prayerGroupUserSummaries };
        }

        public async Task UpdatePrayerGroupAdminsAsync(string authHeader, int prayerGroupId, UpdatePrayerGroupAdminsRequest updateAdminsRequest) {
            if (await IsPrayerGroupAdminAsync(authHeader, prayerGroupId)) {
                throw new ArgumentException("User must be an admin to update prayer group admins.");
            }

            IEnumerable<PrayerGroupUserEntity> prayerGroupUsers = await _prayerGroupRepository.GetPrayerGroupUsersAsync(prayerGroupId, [PrayerGroupRole.Admin]);
            IEnumerable<int> currentAdminUserIds = prayerGroupUsers.Select(user => user.Id ?? -1);
            HashSet<int> currentAdminUserIdsSet = new HashSet<int>(currentAdminUserIds);
            HashSet<int> updatedAdminUserIdsSet = new HashSet<int>(updateAdminsRequest.UserIds);

            IEnumerable<int> adminsToRemove = currentAdminUserIdsSet.Except(updatedAdminUserIdsSet);
            IEnumerable<int> adminsToAdd = updatedAdminUserIdsSet.Except(currentAdminUserIdsSet);
            await _prayerGroupRepository.UpdatePrayerGroupAdminsAsync(prayerGroupId, adminsToAdd, adminsToRemove);
        }

        public async Task AddPrayerGroupUsersAsync(int prayerGroupId, AddPrayerGroupUserRequest request) {
            IEnumerable<PrayerGroupUserToAdd> usersToAdd = _mapper.Map<IEnumerable<PrayerGroupUserToAdd>>(request.Users);
            await _prayerGroupRepository.AddPrayerGroupUsersAsync(prayerGroupId, usersToAdd);
        }

        public async Task DeletePrayerGroupUsersAsync(string authHeader, int prayerGroupId, PrayerGroupDeleteRequest request) {
            string username = _userManager.ExtractUsernameFromAuthHeader(authHeader);
            PrayerGroupAppUser? prayerGroupUser = await _prayerGroupRepository.GetPrayerGroupAppUserByUsernameAsync(prayerGroupId, username);

            if (prayerGroupUser == null) {
                throw new ArgumentException("User must be a member of the prayer group to delete prayer group users.");
            }

            bool isUserDeletingSelf = request.UserIds.ToArray().Length == 1 && request.UserIds.Contains(prayerGroupUser.Id ?? -1);
            if (!isUserDeletingSelf && prayerGroupUser.PrayerGroupRole != PrayerGroupRole.Admin) {
                throw new ArgumentException("User must be an admin to delete prayer group users.");
            }

            await _prayerGroupRepository.DeletePrayerGroupUsersAsync(prayerGroupId, request.UserIds);
        }

        public async Task<bool> IsPrayerGroupAdminAsync(string authHeader, int prayerGroupId) {
            string username = _userManager.ExtractUsernameFromAuthHeader(authHeader);
            PrayerGroupAppUser? prayerGroupUser = await _prayerGroupRepository.GetPrayerGroupAppUserByUsernameAsync(prayerGroupId, username);
            return prayerGroupUser != null && prayerGroupUser.PrayerGroupRole != PrayerGroupRole.Admin;
        }

        private async Task<MediaFile?> GetMediaFileByNullableIdAsync(int? fileId) {
            return fileId.HasValue ? await _mediaFileRepository.GetMediaFileByIdAsync(fileId ?? -1, false) : null;
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

        private static MediaFileBase? GetGroupBannerImageFromCreateResponse(PrayerGroupDetailsEntity response) {
            if (response.BannerImageFileId == null) {
                return null;
            }
            return new MediaFileBase {
                Id = response.BannerImageFileId,
                FileName = response.BannerImageFileName ?? "",
                Url = response.BannerImageFileUrl ?? "",
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
