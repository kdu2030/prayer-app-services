using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUrlToFileUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "url",
                table: "media_files",
                newName: "file_url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "file_url",
                table: "media_files",
                newName: "url");
        }
    }
}
