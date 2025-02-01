using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PrayerAppServices.Data;
using PrayerAppServices.PrayerGroups.Constants;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public class PrayerGroupRepository(AppDbContext dbContext, IConfiguration configuration) : IPrayerGroupRepository {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");



        public async Task<CreatePrayerGroupResponse> CreatePrayerGroupAsync(string adminUsername, NewPrayerGroup newPrayerGroup) {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("admin_username", adminUsername);
            parameters.Add("group_name", newPrayerGroup.GroupName);
            parameters.Add("description", newPrayerGroup.Description);
            parameters.Add("rules", newPrayerGroup.Rules);
            parameters.Add("color", newPrayerGroup.Color);
            parameters.Add("image_file_id", newPrayerGroup.ImageFileId);

            string sql = "SELECT * FROM create_prayer_group(@admin_username, @group_name, @description, @rules, @color, @image_file_id)";
            CreatePrayerGroupResponse response = await connection.QueryFirstAsync<CreatePrayerGroupResponse>(sql, parameters);

            return response;
        }

        public Task<PrayerGroup?> GetPrayerGroupByIdAsync(int id) {
            return _dbContext.PrayerGroups
                .Include(group => group.ImageFile)
                .FirstOrDefaultAsync(group => group.Id == id);
        }

        public async Task<IEnumerable<PrayerGroupUserEntity>> GetPrayerGroupAdminsAsync(int prayerGroupId) {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("prayer_group_id", prayerGroupId);

            string sql = "SELECT * FROM get_prayer_group_admins(@prayer_group_id)";
            IEnumerable<PrayerGroupUserEntity> adminUsers = await connection.QueryAsync<PrayerGroupUserEntity>(sql, parameters);
            return adminUsers;
        }

        public async Task<IEnumerable<PrayerGroupUserEntity>> GetPrayerGroupUsersAsync(int prayerGroupId, IEnumerable<PrayerGroupRole> prayerGroupRoles) {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            PrayerGroupRole[] rolesToQuery = prayerGroupRoles.ToArray();

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("prayer_group_id", prayerGroupId);
            parameters.Add("prayer_group_roles", Array.ConvertAll(rolesToQuery, role => (int)role), System.Data.DbType.Object);

            string sql = "SELECT * FROM get_prayer_group_users(@prayer_group_id, @prayer_group_roles)";
            IEnumerable<PrayerGroupUserEntity> users = await connection.QueryAsync<PrayerGroupUserEntity>(sql, parameters);
            return users;
        }

        public PrayerGroupAppUser? GetPrayerGroupAppUser(int prayerGroupId, string username) {
            FormattableString query = $"SELECT * FROM get_prayer_group_user({prayerGroupId}, {username})";
            return _dbContext.Database.SqlQuery<PrayerGroupAppUser>(query).FirstOrDefault();
        }

        public PrayerGroup? GetPrayerGroupByName(string groupName) {
            return _dbContext.PrayerGroups.Where(group => group.GroupName == groupName)
                .FirstOrDefault();
        }

        public IEnumerable<PrayerGroupSearchResult> SearchPrayerGroupsByName(string nameQuery, int maxNumResults) {
            FormattableString query = $"SELECT * FROM search_prayer_groups_by_name({nameQuery}, {maxNumResults})";
            return _dbContext.Database.SqlQuery<PrayerGroupSearchResult>(query);
        }

    }
}
