using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFiletoUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "image_file_id",
                table: "asp_net_users",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_image_file_id",
                table: "asp_net_users",
                column: "image_file_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_media_files_image_file_id",
                table: "asp_net_users",
                column: "image_file_id",
                principalTable: "media_files",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_media_files_image_file_id",
                table: "asp_net_users");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_image_file_id",
                table: "asp_net_users");

            migrationBuilder.DropColumn(
                name: "image_file_id",
                table: "asp_net_users");
        }
    }
}
