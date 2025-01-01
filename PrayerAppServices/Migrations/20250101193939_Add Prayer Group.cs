using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class AddPrayerGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "prayer_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    rules = table.Column<string>(type: "text", nullable: true),
                    color = table.Column<int>(type: "integer", nullable: true),
                    image_file_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prayer_groups", x => x.id);
                    table.UniqueConstraint("ak_prayer_groups_name", x => x.name);
                    table.ForeignKey(
                        name: "fk_prayer_groups_media_files_image_file_id",
                        column: x => x.image_file_id,
                        principalTable: "media_files",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_prayer_groups_image_file_id",
                table: "prayer_groups",
                column: "image_file_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prayer_groups");
        }
    }
}
