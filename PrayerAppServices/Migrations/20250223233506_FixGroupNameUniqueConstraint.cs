using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class FixGroupNameUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_prayer_groups_group_name",
                table: "prayer_groups",
                column: "group_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_prayer_groups_group_name",
                table: "prayer_groups");
        }
    }
}
