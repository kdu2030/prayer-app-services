using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class AddPrayerGroupUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "prayer_group_users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prayer_group_id = table.Column<int>(type: "integer", nullable: false),
                    app_user_id = table.Column<int>(type: "integer", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prayer_group_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_prayer_group_users_prayer_groups_prayer_group_id",
                        column: x => x.prayer_group_id,
                        principalTable: "prayer_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prayer_group_users_users_app_user_id",
                        column: x => x.app_user_id,
                        principalTable: "asp_net_users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_prayer_group_users_app_user_id",
                table: "prayer_group_users",
                column: "app_user_id");

            migrationBuilder.CreateIndex(
                name: "ix_prayer_group_users_prayer_group_id",
                table: "prayer_group_users",
                column: "prayer_group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prayer_group_users");
        }
    }
}
