using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePrayerGroupImageNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prayer_groups_media_files_banner_image_file_id",
                table: "prayer_groups");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_groups_media_files_image_file_id",
                table: "prayer_groups");

            migrationBuilder.RenameColumn(
                name: "image_file_id",
                table: "prayer_groups",
                newName: "banner_file_id");

            migrationBuilder.RenameColumn(
                name: "banner_image_file_id",
                table: "prayer_groups",
                newName: "avatar_file_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_groups_image_file_id",
                table: "prayer_groups",
                newName: "ix_prayer_groups_banner_file_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_groups_banner_image_file_id",
                table: "prayer_groups",
                newName: "ix_prayer_groups_avatar_file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_groups_media_files_avatar_file_id",
                table: "prayer_groups",
                column: "avatar_file_id",
                principalTable: "media_files",
                principalColumn: "media_file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_groups_media_files_banner_file_id",
                table: "prayer_groups",
                column: "banner_file_id",
                principalTable: "media_files",
                principalColumn: "media_file_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prayer_groups_media_files_avatar_file_id",
                table: "prayer_groups");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_groups_media_files_banner_file_id",
                table: "prayer_groups");

            migrationBuilder.RenameColumn(
                name: "banner_file_id",
                table: "prayer_groups",
                newName: "image_file_id");

            migrationBuilder.RenameColumn(
                name: "avatar_file_id",
                table: "prayer_groups",
                newName: "banner_image_file_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_groups_banner_file_id",
                table: "prayer_groups",
                newName: "ix_prayer_groups_image_file_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_groups_avatar_file_id",
                table: "prayer_groups",
                newName: "ix_prayer_groups_banner_image_file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_groups_media_files_banner_image_file_id",
                table: "prayer_groups",
                column: "banner_image_file_id",
                principalTable: "media_files",
                principalColumn: "media_file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_groups_media_files_image_file_id",
                table: "prayer_groups",
                column: "image_file_id",
                principalTable: "media_files",
                principalColumn: "media_file_id");
        }
    }
}
