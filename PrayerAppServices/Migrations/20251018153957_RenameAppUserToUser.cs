using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class RenameAppUserToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prayer_group_users_users_app_user_id",
                table: "prayer_group_users");

            migrationBuilder.RenameColumn(
                name: "app_user_id",
                table: "prayer_group_users",
                newName: "user_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_group_users_app_user_id",
                table: "prayer_group_users",
                newName: "ix_prayer_group_users_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_group_users_users_user_id",
                table: "prayer_group_users",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prayer_group_users_users_user_id",
                table: "prayer_group_users");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "prayer_group_users",
                newName: "app_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_group_users_user_id",
                table: "prayer_group_users",
                newName: "ix_prayer_group_users_app_user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_group_users_users_app_user_id",
                table: "prayer_group_users",
                column: "app_user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
