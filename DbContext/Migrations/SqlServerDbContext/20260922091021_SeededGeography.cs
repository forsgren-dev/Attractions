using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class SeededGeography : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Countries_CountryName",
                schema: "supusr",
                table: "Countries");

            migrationBuilder.AddColumn<bool>(
                name: "Seeded",
                schema: "supusr",
                table: "Countries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Seeded",
                schema: "supusr",
                table: "Cities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryName_Seeded",
                schema: "supusr",
                table: "Countries",
                columns: new[] { "CountryName", "Seeded" },
                unique: true,
                filter: "[CountryName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Seeded",
                schema: "supusr",
                table: "Countries",
                column: "Seeded");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Seeded",
                schema: "supusr",
                table: "Cities",
                column: "Seeded");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Countries_CountryName_Seeded",
                schema: "supusr",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Countries_Seeded",
                schema: "supusr",
                table: "Countries");

            migrationBuilder.DropIndex(
                name: "IX_Cities_Seeded",
                schema: "supusr",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Seeded",
                schema: "supusr",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Seeded",
                schema: "supusr",
                table: "Cities");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_CountryName",
                schema: "supusr",
                table: "Countries",
                column: "CountryName",
                unique: true,
                filter: "[CountryName] IS NOT NULL");
        }
    }
}
