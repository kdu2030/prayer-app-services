using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGroupNameKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "ak_prayer_groups_group_name",
                table: "prayer_groups");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "ak_prayer_groups_group_name",
                table: "prayer_groups",
                column: "group_name");
        }
    }
}
