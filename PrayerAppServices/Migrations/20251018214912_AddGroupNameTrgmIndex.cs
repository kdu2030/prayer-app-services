using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupNameTrgmIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string path = @"../PrayerAppServices/SQL/group_name_trgm_index.sql";
            migrationBuilder.Sql(File.ReadAllText(path));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX group_name_trgm_idx;");
        }
    }
}
