using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations {
    /// <inheritdoc />
    public partial class CreateSearchPrayerGroupsByName : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            string filePath = @"../PrayerAppServices/SQL/search_prayer_groups_by_name.sql";
            migrationBuilder.Sql(File.ReadAllText(filePath));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS search_prayer_groups_by_name");
        }
    }
}
