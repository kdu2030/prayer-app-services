using AutoMapper;
using PrayerAppServices.Files;
using PrayerAppServices.Files.Constants;
using PrayerAppServices.Files.Entities;
using PrayerAppServices.JoinRequests;
using PrayerAppServices.JoinRequests.Entities;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.DTOs;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Users;
using PrayerAppServices.Users.Models;

namespace PrayerAppServices.PrayerGroups
{
    public class PrayerGroupManager(IPrayerGroupRepository prayerGroupRepository, IUserManager userManager, IMediaFileRepository mediaFileRepository, IJoinRequestRepository joinRequestRepository, IMapper mapper) : IPrayerGroupManager
    {
        private readonly IPrayerGroupRepository _prayerGroupRepository = prayerGroupRepository;
        private readonly IUserManager _userManager = userManager;
        private readonly IMediaFileRepository _mediaFileRepository = mediaFileRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IJoinRequestRepository _joinRequestRepository = joinRequestRepository;

        public async Task<PrayerGroupModel> CreatePrayerGroupAsync(string authToken, PrayerGroupRequest newPrayerGroupRequest)
        {
            int userId = _userManager.ExtractUserIdFromAuthHeader(authToken);
            PrayerGroupDTO newPrayerGroup = new PrayerGroupDTO
            {
                CreatorUserId = userId,
                NewGroupName = newPrayerGroupRequest.GroupName,
                GroupDescription = newPrayerGroupRequest.Description,
                GroupRules = newPrayerGroupRequest.Rules,
                GroupAvatarFileId = newPrayerGroupRequest.AvatarFileId,
                GroupBannerFileId = newPrayerGroupRequest.BannerFileId,
                GroupVisibility = newPrayerGroupRequest.VisibilityLevel ?? VisibilityLevel.Public,
            };

            PrayerGroupDetailsEntity createResponse = await _prayerGroupRepository.CreatePrayerGroupAsync(newPrayerGroup);

            MediaFileBase? groupImage = GetGroupImageFromCreateResponse(createResponse);
            MediaFileBase? bannerImage = GetGroupBannerImageFromCreateResponse(createResponse);

            IEnumerable<PrayerGroupUserSummary>? adminUsers = GetAdminUserFromCreateResponse(createResponse);

            PrayerGroupModel prayerGroupDetails = new PrayerGroupModel
            {
                PrayerGroupId = createResponse.PrayerGroupId,
                GroupName = createResponse.GroupName,
                Description = createResponse.Description,
                Rules = createResponse.Rules,
                AvatarFile = groupImage,
                BannerFile = bannerImage,
                Admins = adminUsers,
                JoinStatus = JoinStatus.Joined,
                PrayerGroupRole = PrayerGroupRole.Admin,
                VisibilityLevel = (VisibilityLevel?)createResponse.VisibilityLevel,
            };

            return prayerGroupDetails;
        }

        public async Task<PrayerGroupModel> GetPrayerGroupDetailsAsync(string authHeader, int prayerGroupId)
        {
            int userId = _userManager.ExtractUserIdFromAuthHeader(authHeader);

            Task<PrayerGroupGetResponse> prayerGroupGetResponseTask = _prayerGroupRepository.GetPrayerGroupAsync(new PrayerGroupQuery { TargetPrayerGroupId = prayerGroupId, TargetUserId = userId });
            Task<IEnumerable<PrayerGroupUserEntity>> prayerGroupAdminEntitiesTask = _prayerGroupRepository.GetPrayerGroupUsersAsync(prayerGroupId, [PrayerGroupRole.Admin]);

            PrayerGroupGetResponse prayerGroupGetResponse = await prayerGroupGetResponseTask;
            IEnumerable<PrayerGroupUserEntity> prayerGroupAdminEntities = await prayerGroupAdminEntitiesTask;

            PrayerGroupModel prayerGroup = _mapper.Map<PrayerGroupModel>(prayerGroupGetResponse);
            IEnumerable<PrayerGroupUserSummary> prayerGroupAdmins = _mapper.Map<IEnumerable<PrayerGroupUserSummary>>(prayerGroupAdminEntities);

            if (prayerGroupGetResponse.PrayerGroupRole.HasValue)
            {
                prayerGroup.JoinStatus = JoinStatus.Joined;
            }
            else if (prayerGroupGetResponse.JoinRequestId.HasValue)
            {
                prayerGroup.JoinStatus = JoinStatus.RequestSubmitted;
            }
            else
            {
                prayerGroup.JoinStatus = JoinStatus.NotJoined;
            }

            prayerGroup.Admins = prayerGroupAdmins;

            return prayerGroup;
        }

        public async Task<GroupNameValidationResponse> ValidateGroupNameAsync(string groupName)
        {
            List<string> errors = new List<string>();
            PrayerGroup? prayerGroup = await _prayerGroupRepository.GetPrayerGroupByNameAsync(groupName);
            if (prayerGroup != null)
            {
                errors.Add("A prayer group with this name already exists.");
            }

            return new GroupNameValidationResponse { IsNameValid = errors.Count == 0, Errors = errors };
        }

        public async Task<IEnumerable<PrayerGroupModel>> SearchPrayerGroupsAsync(PrayerGroupSearchRequest prayerGroupSearchRequest)
        {
            IEnumerable<PrayerGroupSearchResult> searchResults = await _prayerGroupRepository.SearchPrayerGroupsAsync(prayerGroupSearchRequest.GroupNameQuery, prayerGroupSearchRequest.MaxNumResults ?? 20);
            return _mapper.Map<IEnumerable<PrayerGroupModel>>(searchResults);
        }

        public async Task<PrayerGroupModel> UpdatePrayerGroupAsync(string authToken, int prayerGroupId, PrayerGroupRequest prayerGroupRequest)
        {
            int userId = _userManager.ExtractUserIdFromAuthHeader(authToken);
            PrayerGroupUser? prayerGroupUser = await _prayerGroupRepository.GetPrayerGroupUserByUserIdAsync(userId, prayerGroupId);

            if (prayerGroupUser == null || prayerGroupUser.PrayerGroupRole != PrayerGroupRole.Admin)
            {
                throw new ArgumentException(PrayerGroupValidationErrors.MustBeAdminToModifyPrayerGroup);
            }

            if (prayerGroupRequest.VisibilityLevel == VisibilityLevel.Public)
            {
                IEnumerable<JoinRequest> joinRequests = await _joinRequestRepository.GetJoinRequestsAsync(prayerGroupId, 0, 1);
                if (joinRequests.Count() > 0)
                {
                    throw new ArgumentException(PrayerGroupValidationErrors.CannotBePublicWithActiveJoinRequests);
                }
            }



            int? imageFileId = prayerGroupRequest.AvatarFileId;
            int? bannerImageFileId = prayerGroupRequest.BannerFileId;


            MediaFile? groupImageFile = await GetMediaFileByNullableIdAsync(imageFileId);
            MediaFile? bannerImageFile = await GetMediaFileByNullableIdAsync(bannerImageFileId);

            if (groupImageFile != null && groupImageFile.FileType != FileType.Image)
            {
                throw new ArgumentException("Cannot use a non-image as a prayer group image");
            }

            if (bannerImageFile != null && bannerImageFile.FileType != FileType.Image)
            {
                throw new ArgumentException("Cannot use a non-image as a prayer group banner image.");
            }

            PrayerGroup updatedPrayerGroup = _mapper.Map<PrayerGroup>(prayerGroupRequest, opts =>
            {
                opts.Items["Id"] = prayerGroupId;
                opts.Items["ImageFile"] = groupImageFile;
                opts.Items["BannerImageFile"] = bannerImageFile;
            });

            await _prayerGroupRepository.UpdatePrayerGroupAsync(updatedPrayerGroup);
            return _mapper.Map<PrayerGroupModel>(updatedPrayerGroup);
        }

        public async Task<PrayerGroupUsersResponse> GetPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupRole>? prayerGroupRoles)
        {
            IEnumerable<PrayerGroupRole> rolesToSearch = prayerGroupRoles == null || prayerGroupRoles.Count() == 0 ? [PrayerGroupRole.Member, PrayerGroupRole.Admin] : prayerGroupRoles;
            IEnumerable<PrayerGroupUserEntity> prayerGroupUsers = await _prayerGroupRepository.GetPrayerGroupUsersAsync(prayerGroupId, rolesToSearch);
            IEnumerable<PrayerGroupUserSummary> prayerGroupUserSummaries = _mapper.Map<IEnumerable<PrayerGroupUserSummary>>(prayerGroupUsers);
            return new PrayerGroupUsersResponse { Users = prayerGroupUserSummaries };
        }

        public async Task UpdatePrayerGroupAdminsAsync(string authHeader, int prayerGroupId, UpdatePrayerGroupAdminsRequest updateAdminsRequest)
        {
            if (await IsPrayerGroupAdminAsync(authHeader, prayerGroupId))
            {
                throw new ArgumentException("User must be an admin to update prayer group admins.");
            }

            IEnumerable<PrayerGroupUserEntity> prayerGroupUsers = await _prayerGroupRepository.GetPrayerGroupUsersAsync(prayerGroupId, [PrayerGroupRole.Admin]);
            IEnumerable<int> currentAdminUserIds = prayerGroupUsers.Select(user => user.UserId ?? -1);
            HashSet<int> currentAdminUserIdsSet = new HashSet<int>(currentAdminUserIds);
            HashSet<int> updatedAdminUserIdsSet = new HashSet<int>(updateAdminsRequest.UserIds);

            IEnumerable<int> adminsToRemove = currentAdminUserIdsSet.Except(updatedAdminUserIdsSet);
            IEnumerable<int> adminsToAdd = updatedAdminUserIdsSet.Except(currentAdminUserIdsSet);
            await _prayerGroupRepository.UpdatePrayerGroupAdminsAsync(prayerGroupId, adminsToAdd, adminsToRemove);
        }

        public async Task AddPrayerGroupUsersAsync(int prayerGroupId, AddPrayerGroupUserRequest request)
        {
            IEnumerable<PrayerGroupUserToAdd> usersToAdd = _mapper.Map<IEnumerable<PrayerGroupUserToAdd>>(request.Users);
            await _prayerGroupRepository.AddPrayerGroupUsersAsync(prayerGroupId, usersToAdd);
        }

        public async Task DeletePrayerGroupUsersAsync(string authHeader, int prayerGroupId, PrayerGroupDeleteRequest request)
        {
            string username = _userManager.ExtractUsernameFromAuthHeader(authHeader);
            PrayerGroupAppUser? prayerGroupUser = await _prayerGroupRepository.GetPrayerGroupAppUserByUsernameAsync(prayerGroupId, username);

            if (prayerGroupUser == null)
            {
                throw new ArgumentException("User must be a member of the prayer group to delete prayer group users.");
            }

            bool isUserDeletingSelf = request.UserIds.ToArray().Length == 1 && request.UserIds.Contains(prayerGroupUser.UserId ?? -1);
            if (!isUserDeletingSelf && prayerGroupUser.PrayerGroupRole != PrayerGroupRole.Admin)
            {
                throw new ArgumentException("User must be an admin to delete prayer group users.");
            }

            await _prayerGroupRepository.DeletePrayerGroupUsersAsync(prayerGroupId, request.UserIds);
        }

        public async Task<bool> IsPrayerGroupAdminAsync(string authHeader, int prayerGroupId)
        {
            string username = _userManager.ExtractUsernameFromAuthHeader(authHeader);
            PrayerGroupAppUser? prayerGroupUser = await _prayerGroupRepository.GetPrayerGroupAppUserByUsernameAsync(prayerGroupId, username);
            return prayerGroupUser != null && prayerGroupUser.PrayerGroupRole != PrayerGroupRole.Admin;
        }

        private async Task<MediaFile?> GetMediaFileByNullableIdAsync(int? fileId)
        {
            return fileId.HasValue ? await _mediaFileRepository.GetMediaFileByIdAsync(fileId ?? -1, false) : null;
        }

        private static MediaFileBase? GetGroupImageFromCreateResponse(PrayerGroupDetailsEntity response)
        {
            if (response.AvatarFileId == null)
            {
                return null;
            }
            return new MediaFileBase
            {
                MediaFileId = response.AvatarFileId,
                FileName = response.GroupAvatarFileName ?? "",
                FileUrl = response.GroupAvatarFileUrl ?? "",
                FileType = FileType.Image,
            };
        }

        private static MediaFileBase? GetGroupBannerImageFromCreateResponse(PrayerGroupDetailsEntity response)
        {
            if (response.BannerFileId == null)
            {
                return null;
            }
            return new MediaFileBase
            {
                MediaFileId = response.BannerFileId,
                FileName = response.GroupBannerFileName ?? "",
                FileUrl = response.GroupBannerFileUrl ?? "",
                FileType = FileType.Image,
            };
        }

        private static IEnumerable<PrayerGroupUserSummary>? GetAdminUserFromCreateResponse(PrayerGroupDetailsEntity response)
        {
            if (response.AdminUserId == null)
            {
                return null;
            }

            MediaFileBase? userImage = response.AdminImageFileId != null ?
                new MediaFileBase
                {
                    MediaFileId = response.AdminImageFileId,
                    FileName = response.AdminImageFileName ?? "",
                    FileUrl = response.AdminImageFileUrl ?? "",
                    FileType = FileType.Image
                }
                : null;

            PrayerGroupUserSummary adminUserSummary = new PrayerGroupUserSummary
            {
                UserId = response.AdminUserId ?? -1,
                FullName = response.AdminFullName,
                Image = userImage,
                PrayerGroupRole = PrayerGroupRole.Admin,
            };

            return [adminUserSummary];
        }

        private IEnumerable<UserSummary> GetAdminUserSummaries(IEnumerable<PrayerGroupUserEntity> adminUsers)
        {
            return adminUsers.Where(adminUser => adminUser.UserId != null)
                .Select(adminUser => new UserSummary
                {
                    UserId = adminUser.UserId ?? -1,
                    FullName = adminUser.FullName,
                    Image = adminUser.ImageFileId != null ? new MediaFileBase
                    {
                        MediaFileId = adminUser.ImageFileId,
                        FileName = adminUser.FileName ?? "",
                        FileUrl = adminUser.FileUrl ?? "",
                        FileType = FileType.Image
                    } : null
                });
        }

    }
}
