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

        public async Task<PrayerGroup?> GetPrayerGroupByIdAsync(int id) {
            return await _dbContext.PrayerGroups.FindAsync([id]);
        }

        public async Task<IQueryable<PrayerGroupAdminUser>> GetPrayerGroupAdminsAsync(int prayerGroupId) {
            FormattableString query = @$"SELECT * FROM get_prayer_group_admins({prayerGroupId})";
            Task<IQueryable<PrayerGroupAdminUser>> task = Task.FromResult(_dbContext.Database.SqlQuery<PrayerGroupAdminUser>(query));
            return await task;
        }

    }
}
