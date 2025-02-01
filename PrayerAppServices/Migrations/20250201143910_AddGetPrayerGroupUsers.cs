using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations {
    /// <inheritdoc />
    public partial class AddGetPrayerGroupUsers : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            string filePath = @"../PrayerAppServices/SQL/get_prayer_group_users.sql";
            migrationBuilder.Sql(File.ReadAllText(filePath));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS get_prayer_group_users");
        }
    }
}
