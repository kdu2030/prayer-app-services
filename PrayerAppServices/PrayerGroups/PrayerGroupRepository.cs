using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PrayerAppServices.Data;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.DTOs;
using PrayerAppServices.PrayerGroups.Entities;

namespace PrayerAppServices.PrayerGroups
{
    public class PrayerGroupRepository(AppDbContext dbContext, NpgsqlDataSource dataSource) : IPrayerGroupRepository
    {
        private readonly AppDbContext _dbContext = dbContext;

        private ValueTask<NpgsqlConnection> Connection
        {
            get
            {
                return dataSource.OpenConnectionAsync();
            }
        }

        public async Task<PrayerGroupDetailsEntity> CreatePrayerGroupAsync(PrayerGroupDTO newPrayerGroup)
        {
            await using NpgsqlConnection connection = await Connection;
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("creator_user_id", newPrayerGroup.CreatorUserId);
            parameters.Add("new_group_name", newPrayerGroup.NewGroupName);
            parameters.Add("group_description", newPrayerGroup.GroupDescription);
            parameters.Add("group_rules", newPrayerGroup.GroupRules);
            parameters.Add("group_visibility", newPrayerGroup.GroupVisibility);
            parameters.Add("group_avatar_file_id", newPrayerGroup.GroupAvatarFileId);
            parameters.Add("group_banner_file_id", newPrayerGroup.GroupBannerFileId);

            string sql = "SELECT * FROM create_prayer_group(@creator_user_id, @new_group_name, @group_description, @group_rules, @group_visibility, @group_avatar_file_id, @group_banner_file_id)";
            PrayerGroupDetailsEntity response = await connection.QueryFirstAsync<PrayerGroupDetailsEntity>(sql, parameters);

            return response;
        }

        public Task<PrayerGroup?> GetPrayerGroupByIdAsync(int id, bool includeImage = false)
        {
            if (includeImage)
            {
                return _dbContext.PrayerGroups
                    .Include(group => group.AvatarFile)
                    .Include(group => group.BannerFile)
                    .FirstOrDefaultAsync(group => group.PrayerGroupId == id);
            }

            return _dbContext.PrayerGroups
                .FirstOrDefaultAsync(group => group.PrayerGroupId == id);
        }

        public async Task<PrayerGroupGetResponse> GetPrayerGroupAsync(PrayerGroupQuery prayerGroupQuery)
        {
            await using NpgsqlConnection connection = await Connection;
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@target_prayer_group_id", prayerGroupQuery.TargetPrayerGroupId);
            parameters.Add("@target_user_id", prayerGroupQuery.TargetUserId);

            string sql = "SELECT * FROM get_prayer_group(@target_prayer_group_id, @target_user_id);";
            PrayerGroupGetResponse prayerGroupGetResponse = await connection.QueryFirstAsync<PrayerGroupGetResponse>(sql, parameters);

            return prayerGroupGetResponse;
        }

        public async Task<IEnumerable<PrayerGroupUserEntity>> GetPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupRole> prayerGroupRoles)
        {
            await using NpgsqlConnection connection = await Connection;
            PrayerGroupRole[] rolesToQuery = prayerGroupRoles.ToArray();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("prayer_group_id", prayerGroupId);
            parameters.Add("prayer_group_roles", Array.ConvertAll(rolesToQuery, role => (int)role), System.Data.DbType.Object);

            string sql = "SELECT * FROM get_prayer_group_users(@prayer_group_id, @prayer_group_roles)";
            IEnumerable<PrayerGroupUserEntity> users = await connection.QueryAsync<PrayerGroupUserEntity>(sql, parameters);
            return users;
        }

        public async Task<PrayerGroupAppUser?> GetPrayerGroupAppUserByUsernameAsync(int prayerGroupId, string username)
        {
            await using NpgsqlConnection connection = await Connection;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("prayer_group_id", prayerGroupId);
            parameters.Add("username", username);

            string sql = "SELECT * FROM get_prayer_group_user(@prayer_group_id, @username)";
            PrayerGroupAppUser? appUser = await connection.QueryFirstOrDefaultAsync<PrayerGroupAppUser>(sql, parameters);
            return appUser;
        }

        public Task<PrayerGroup?> GetPrayerGroupByNameAsync(string groupName, bool enableTracking = true)
        {
            if (enableTracking)
            {
                return _dbContext.PrayerGroups.Where(group => group.GroupName == groupName)
               .FirstOrDefaultAsync();
            }

            return _dbContext.PrayerGroups.Where(group => group.GroupName == groupName)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public IEnumerable<PrayerGroupSearchResult> SearchPrayerGroupsByName(string nameQuery, int maxNumResults)
        {
            FormattableString query = $"SELECT * FROM search_prayer_groups_by_name({nameQuery}, {maxNumResults})";
            return _dbContext.Database.SqlQuery<PrayerGroupSearchResult>(query);
        }

        public async Task UpdatePrayerGroupAsync(PrayerGroup prayerGroup)
        {
            _dbContext.PrayerGroups.Update(prayerGroup);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<PrayerGroupSummaryEntity>> GetPrayerGroupSummariesByUserIdAsync(int userId)
        {
            await using NpgsqlConnection connection = await Connection;
            DynamicParameters dynamicParams = new DynamicParameters();
            dynamicParams.Add("user_id", userId);

            string sql = "SELECT * FROM get_prayer_group_summaries_by_user(@user_id)";
            IEnumerable<PrayerGroupSummaryEntity> prayerGroupSummaries = await connection.QueryAsync<PrayerGroupSummaryEntity>(sql, dynamicParams);
            return prayerGroupSummaries;
        }

        public async Task UpdatePrayerGroupAdminsAsync(int prayerGroupId, IEnumerable<int> adminUserIdsToAdd, IEnumerable<int> adminUserIdsToRemove)
        {
            await using NpgsqlConnection connection = await Connection;

            int[] adminUserIdsToAddArr = adminUserIdsToAdd.ToArray();
            int[] adminUsersToRemoveIdArr = adminUserIdsToRemove.ToArray();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("prayer_group_id", prayerGroupId);
            parameters.Add("admin_user_ids_to_add", adminUserIdsToAddArr.Length > 0 ? adminUserIdsToAddArr : null, System.Data.DbType.Object);
            parameters.Add("admin_user_ids_to_remove", adminUsersToRemoveIdArr.Length > 0 ? adminUsersToRemoveIdArr : null, System.Data.DbType.Object);

            string sql = "CALL update_prayer_group_admins(@prayer_group_id, @admin_user_ids_to_add, @admin_user_ids_to_remove)";
            await connection.ExecuteAsync(sql, parameters);
        }

        public async Task AddPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupUserToAdd> users)
        {
            await using NpgsqlConnection connection = await Connection;

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("prayer_group_id", prayerGroupId);
            parameters.Add("users_to_add", users.ToArray(), System.Data.DbType.Object);

            string sql = "CALL add_prayer_group_users(@prayer_group_id, @users_to_add)";
            await connection.ExecuteAsync(sql, parameters);
        }

        public async Task DeletePrayerGroupUsersAsync(int prayerGroupId, IEnumerable<int> userIds)
        {
            _dbContext.PrayerGroupUsers.RemoveRange(
                _dbContext.PrayerGroupUsers.Where(user => user.PrayerGroup.PrayerGroupId == prayerGroupId && userIds.Contains(user.User.Id))
            );
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PrayerGroupUser?> GetPrayerGroupUserByUserIdAsync(int prayerGroupId, int userId, CancellationToken token = default)
        {
            return await _dbContext.PrayerGroupUsers
                .Where(user => user.PrayerGroup.PrayerGroupId == prayerGroupId && user.User.Id == userId)
                .FirstOrDefaultAsync(token);
        }
    }
}
