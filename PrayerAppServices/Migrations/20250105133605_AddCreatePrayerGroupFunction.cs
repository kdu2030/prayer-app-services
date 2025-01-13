using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations {
    /// <inheritdoc />
    public partial class AddCreatePrayerGroupFunction : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            string filePath = @"../PrayerAppServices/SQL/create_prayer_group.sql";
            migrationBuilder.Sql(File.ReadAllText(filePath));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS create_prayer_group");

        }
    }
}
