using PrayerAppServices.Files.Entities;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Users;
using PrayerAppServices.Utils;

namespace PrayerAppServices.PrayerGroups {
    public class PrayerGroupManager(IPrayerGroupRepository prayerGroupRepository, IUserManager userManager) {
        private readonly IPrayerGroupRepository _prayerGroupRepository = prayerGroupRepository;
        private readonly IUserManager _userManager = userManager;

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

            // TODO: Missing Image and Admins Mapping

            PrayerGroupDetails prayerGroupDetails = new PrayerGroupDetails {
                Id = createResponse.Id,
                Name = createResponse.Name,
                Description = createResponse.Description,
                Rules = createResponse.Rules,
                Color = colorStr,
                IsUserJoined = true,
                IsUserAdmin = true,
            };

            throw new NotImplementedException();
        }

    }
}
