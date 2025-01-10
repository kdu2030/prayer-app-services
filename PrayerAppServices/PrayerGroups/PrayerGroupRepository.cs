using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PrayerAppServices.Data;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;
using PrayerAppServices.Users.Entities;
using System.Text.RegularExpressions;

namespace PrayerAppServices.PrayerGroups {
    public class PrayerGroupRepository(AppDbContext dbContext) : IPrayerGroupRepository {
        private readonly AppDbContext _dbContext = dbContext;

        public CreatePrayerGroupResponse CreatePrayerGroup(string adminUsername, NewPrayerGroup newPrayerGroup) {
            FormattableString sqlQuery = $@"SELECT * FROM create_prayer_group(
                {adminUsername},
                {newPrayerGroup.Name},
                {newPrayerGroup.Description},
                {newPrayerGroup.Rules},
                {newPrayerGroup.Color},
                {newPrayerGroup.ImageFileId}
            )";


            return _dbContext.Database.SqlQuery<CreatePrayerGroupResponse>(
               sqlQuery
            ).First();
        }

        public PrayerGroup? GetPrayerGroupById(int id) {
            return _dbContext.PrayerGroups
                .Include(group => group.ImageFile)
                .FirstOrDefault(group => group.Id == id);
        }

        public IQueryable<PrayerGroupAdminUser> GetPrayerGroupAdmins(int prayerGroupId) {
            FormattableString query = $"SELECT * FROM get_prayer_group_admins({prayerGroupId})";
            return _dbContext.Database.SqlQuery<PrayerGroupAdminUser>(query);
        }

        public PrayerGroupAppUser? GetPrayerGroupAppUser(int prayerGroupId, string username) {
            FormattableString query = $"SELECT * FROM get_prayer_group_user({prayerGroupId}, {username})";
            return _dbContext.Database.SqlQuery<PrayerGroupAppUser>(query).FirstOrDefault();
        }

    }
}
