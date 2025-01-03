using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrayerAppServices.Migrations {
    /// <inheritdoc />
    public partial class AddValidateFileDeleteFunction : Migration {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder) {
            string sqlFilePath = @"../PrayerAppServices/SQL/validate_file_delete.sql";
            migrationBuilder.Sql(File.ReadAllText(sqlFilePath));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS validate_file_delete;");
        }

    }
}
