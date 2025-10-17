using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrayerGroupIDs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "prayer_groups",
                newName: "prayer_group_id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "prayer_group_users",
                newName: "prayer_group_user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "prayer_group_id",
                table: "prayer_groups",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "prayer_group_user_id",
                table: "prayer_group_users",
                newName: "id");
        }
    }
}
