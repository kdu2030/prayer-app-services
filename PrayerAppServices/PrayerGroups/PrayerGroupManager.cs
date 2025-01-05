using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Users;

namespace PrayerAppServices.PrayerGroups {
    public class PrayerGroupManager(IPrayerGroupRepository prayerGroupRepository, IUserManager userManager) {
        private readonly IPrayerGroupRepository _prayerGroupRepository = prayerGroupRepository;
        private readonly IUserManager _userManager = userManager;

        public PrayerGroupDetails CreatePrayerGroup(string authToken, NewPrayerGroupRequest newPrayerGroupRequest) {
            string username = _userManager.ExtractUsernameFromToken(authToken);
            throw new NotImplementedException();
        }

    }
}
