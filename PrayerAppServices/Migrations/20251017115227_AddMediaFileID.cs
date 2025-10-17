using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaFileID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_media_files_image_file_id",
                table: "asp_net_users");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "media_files",
                newName: "media_file_id");

            migrationBuilder.RenameColumn(
                name: "image_file_id",
                table: "asp_net_users",
                newName: "image_file_media_file_id");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_users_image_file_id",
                table: "asp_net_users",
                newName: "ix_asp_net_users_image_file_media_file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_media_files_image_file_media_file_id",
                table: "asp_net_users",
                column: "image_file_media_file_id",
                principalTable: "media_files",
                principalColumn: "media_file_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_media_files_image_file_media_file_id",
                table: "asp_net_users");

            migrationBuilder.RenameColumn(
                name: "media_file_id",
                table: "media_files",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "image_file_media_file_id",
                table: "asp_net_users",
                newName: "image_file_id");

            migrationBuilder.RenameIndex(
                name: "ix_asp_net_users_image_file_media_file_id",
                table: "asp_net_users",
                newName: "ix_asp_net_users_image_file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_media_files_image_file_id",
                table: "asp_net_users",
                column: "image_file_id",
                principalTable: "media_files",
                principalColumn: "id");
        }
    }
}
