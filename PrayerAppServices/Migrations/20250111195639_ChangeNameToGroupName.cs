using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations {
    /// <inheritdoc />
    public partial class ChangeNameToGroupName : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropUniqueConstraint(
                name: "ak_prayer_groups_name",
                table: "prayer_groups");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "prayer_groups",
                newName: "group_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.RenameColumn(
                name: "group_name",
                table: "prayer_groups",
                newName: "name");

            migrationBuilder.AddUniqueConstraint(
                name: "ak_prayer_groups_name",
                table: "prayer_groups",
                column: "name");
        }
    }
}
