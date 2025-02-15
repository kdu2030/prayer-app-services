using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class AddPrayerGroupBannerImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "banner_image_file_id",
                table: "prayer_groups",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_prayer_groups_banner_image_file_id",
                table: "prayer_groups",
                column: "banner_image_file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_groups_media_files_banner_image_file_id",
                table: "prayer_groups",
                column: "banner_image_file_id",
                principalTable: "media_files",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prayer_groups_media_files_banner_image_file_id",
                table: "prayer_groups");

            migrationBuilder.DropIndex(
                name: "ix_prayer_groups_banner_image_file_id",
                table: "prayer_groups");

            migrationBuilder.DropColumn(
                name: "banner_image_file_id",
                table: "prayer_groups");
        }
    }
}
