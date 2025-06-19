using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class AddDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_prayer_groups_prayer_group_id",
                table: "prayer_request");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_users_user_id",
                table: "prayer_request");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_bookmark_prayer_request_prayer_request_id",
                table: "prayer_request_bookmark");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_comment_prayer_request_prayer_request_id",
                table: "prayer_request_comment");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_comment_users_user_id",
                table: "prayer_request_comment");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_like_prayer_request_prayer_request_id",
                table: "prayer_request_like");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_like_users_user_id",
                table: "prayer_request_like");

            migrationBuilder.DropPrimaryKey(
                name: "pk_prayer_request_like",
                table: "prayer_request_like");

            migrationBuilder.DropPrimaryKey(
                name: "pk_prayer_request_comment",
                table: "prayer_request_comment");

            migrationBuilder.DropPrimaryKey(
                name: "pk_prayer_request",
                table: "prayer_request");

            migrationBuilder.RenameTable(
                name: "prayer_request_like",
                newName: "prayer_request_likes");

            migrationBuilder.RenameTable(
                name: "prayer_request_comment",
                newName: "prayer_request_comments");

            migrationBuilder.RenameTable(
                name: "prayer_request",
                newName: "prayer_requests");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_request_like_user_id",
                table: "prayer_request_likes",
                newName: "ix_prayer_request_likes_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_request_like_prayer_request_id",
                table: "prayer_request_likes",
                newName: "ix_prayer_request_likes_prayer_request_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_request_comment_user_id",
                table: "prayer_request_comments",
                newName: "ix_prayer_request_comments_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_request_comment_prayer_request_id",
                table: "prayer_request_comments",
                newName: "ix_prayer_request_comments_prayer_request_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_request_user_id",
                table: "prayer_requests",
                newName: "ix_prayer_requests_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_request_prayer_group_id",
                table: "prayer_requests",
                newName: "ix_prayer_requests_prayer_group_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_prayer_request_likes",
                table: "prayer_request_likes",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_prayer_request_comments",
                table: "prayer_request_comments",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_prayer_requests",
                table: "prayer_requests",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_bookmark_prayer_requests_prayer_request_id",
                table: "prayer_request_bookmark",
                column: "prayer_request_id",
                principalTable: "prayer_requests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_comments_prayer_requests_prayer_request_id",
                table: "prayer_request_comments",
                column: "prayer_request_id",
                principalTable: "prayer_requests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_comments_users_user_id",
                table: "prayer_request_comments",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_likes_prayer_requests_prayer_request_id",
                table: "prayer_request_likes",
                column: "prayer_request_id",
                principalTable: "prayer_requests",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_likes_users_user_id",
                table: "prayer_request_likes",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_requests_prayer_groups_prayer_group_id",
                table: "prayer_requests",
                column: "prayer_group_id",
                principalTable: "prayer_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_requests_users_user_id",
                table: "prayer_requests",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_bookmark_prayer_requests_prayer_request_id",
                table: "prayer_request_bookmark");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_comments_prayer_requests_prayer_request_id",
                table: "prayer_request_comments");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_comments_users_user_id",
                table: "prayer_request_comments");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_likes_prayer_requests_prayer_request_id",
                table: "prayer_request_likes");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_request_likes_users_user_id",
                table: "prayer_request_likes");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_requests_prayer_groups_prayer_group_id",
                table: "prayer_requests");

            migrationBuilder.DropForeignKey(
                name: "fk_prayer_requests_users_user_id",
                table: "prayer_requests");

            migrationBuilder.DropPrimaryKey(
                name: "pk_prayer_requests",
                table: "prayer_requests");

            migrationBuilder.DropPrimaryKey(
                name: "pk_prayer_request_likes",
                table: "prayer_request_likes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_prayer_request_comments",
                table: "prayer_request_comments");

            migrationBuilder.RenameTable(
                name: "prayer_requests",
                newName: "prayer_request");

            migrationBuilder.RenameTable(
                name: "prayer_request_likes",
                newName: "prayer_request_like");

            migrationBuilder.RenameTable(
                name: "prayer_request_comments",
                newName: "prayer_request_comment");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_requests_user_id",
                table: "prayer_request",
                newName: "ix_prayer_request_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_requests_prayer_group_id",
                table: "prayer_request",
                newName: "ix_prayer_request_prayer_group_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_request_likes_user_id",
                table: "prayer_request_like",
                newName: "ix_prayer_request_like_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_request_likes_prayer_request_id",
                table: "prayer_request_like",
                newName: "ix_prayer_request_like_prayer_request_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_request_comments_user_id",
                table: "prayer_request_comment",
                newName: "ix_prayer_request_comment_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_prayer_request_comments_prayer_request_id",
                table: "prayer_request_comment",
                newName: "ix_prayer_request_comment_prayer_request_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_prayer_request",
                table: "prayer_request",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_prayer_request_like",
                table: "prayer_request_like",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_prayer_request_comment",
                table: "prayer_request_comment",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_prayer_groups_prayer_group_id",
                table: "prayer_request",
                column: "prayer_group_id",
                principalTable: "prayer_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_users_user_id",
                table: "prayer_request",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_bookmark_prayer_request_prayer_request_id",
                table: "prayer_request_bookmark",
                column: "prayer_request_id",
                principalTable: "prayer_request",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_comment_prayer_request_prayer_request_id",
                table: "prayer_request_comment",
                column: "prayer_request_id",
                principalTable: "prayer_request",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_comment_users_user_id",
                table: "prayer_request_comment",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_like_prayer_request_prayer_request_id",
                table: "prayer_request_like",
                column: "prayer_request_id",
                principalTable: "prayer_request",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prayer_request_like_users_user_id",
                table: "prayer_request_like",
                column: "user_id",
                principalTable: "asp_net_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
