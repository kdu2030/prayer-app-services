using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class DeletePrayerRequestLike : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prayer_request_like");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "prayer_request_like",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    app_user_id = table.Column<int>(type: "integer", nullable: false),
                    prayer_request_id = table.Column<int>(type: "integer", nullable: false)
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
                name: "ix_prayer_request_like_app_user_id",
                table: "prayer_request_like",
                column: "app_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_prayer_request_like_prayer_request_id",
                table: "prayer_request_like",
                column: "prayer_request_id");
        }
    }
}
