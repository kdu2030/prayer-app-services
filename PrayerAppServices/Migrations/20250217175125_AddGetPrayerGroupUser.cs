using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations {
    /// <inheritdoc />
    public partial class AddGetPrayerGroupUser : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            string filePath = @"../PrayerAppServices/SQL/AddGetPrayerGroupUser.sql";
            migrationBuilder.Sql(File.ReadAllText(filePath));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_prayer_group_user");
        }
    }
}
