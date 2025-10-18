using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class AddJoinRequestEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "join_requests",
                columns: table => new
                {
                    join_request_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<int>(type: "integer", nullable: true),
                    prayer_group_id = table.Column<int>(type: "integer", nullable: true),
                    submitted_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_join_requests", x => x.join_request_id);
                    table.ForeignKey(
                        name: "fk_join_requests_prayer_groups_prayer_group_id",
                        column: x => x.prayer_group_id,
                        principalTable: "prayer_groups",
                        principalColumn: "prayer_group_id");
                    table.ForeignKey(
                        name: "fk_join_requests_users_user_id",
                        column: x => x.user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_join_requests_prayer_group_id",
                table: "join_requests",
                column: "prayer_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_join_requests_user_id",
                table: "join_requests",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "join_requests");
        }
    }
}
