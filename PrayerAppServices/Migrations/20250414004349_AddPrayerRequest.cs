using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class AddPrayerRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "prayer_request",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    request_title = table.Column<string>(type: "varchar(255)", nullable: false),
                    request_description = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    prayer_group_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    like_count = table.Column<int>(type: "integer", nullable: false),
                    comment_count = table.Column<int>(type: "integer", nullable: false),
                    prayed_count = table.Column<int>(type: "integer", nullable: false),
                    expiration_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prayer_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_prayer_request_prayer_groups_prayer_group_id",
                        column: x => x.prayer_group_id,
                        principalTable: "prayer_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prayer_request_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prayer_request_comment",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prayer_request_id = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    comment = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prayer_request_comment", x => x.id);
                    table.ForeignKey(
                        name: "fk_prayer_request_comment_prayer_request_prayer_request_id",
                        column: x => x.prayer_request_id,
                        principalTable: "prayer_request",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prayer_request_comment_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prayer_request_like",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prayer_request_id = table.Column<int>(type: "integer", nullable: false),
                    app_user_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prayer_request_like", x => x.id);
                    table.ForeignKey(
                        name: "fk_prayer_request_like_prayer_request_prayer_request_id",
                        column: x => x.prayer_request_id,
                        principalTable: "prayer_request",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prayer_request_like_users_app_user_id",
                        column: x => x.app_user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_prayer_request_prayer_group_id",
                table: "prayer_request",
                column: "prayer_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_prayer_request_user_id",
                table: "prayer_request",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_prayer_request_comment_prayer_request_id",
                table: "prayer_request_comment",
                column: "prayer_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_prayer_request_comment_user_id",
                table: "prayer_request_comment",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_prayer_request_like_app_user_id",
                table: "prayer_request_like",
                column: "app_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_prayer_request_like_prayer_request_id",
                table: "prayer_request_like",
                column: "prayer_request_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prayer_request_comment");

            migrationBuilder.DropTable(
                name: "prayer_request_like");

            migrationBuilder.DropTable(
                name: "prayer_request");
        }
    }
}
