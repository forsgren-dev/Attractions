using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class UniqueAddressByStreetPostalCodeAndCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Street_PostalCode_CityDbMCityId",
                schema: "supusr",
                table: "Addresses",
                columns: new[] { "Street", "PostalCode", "CityDbMCityId" },
                unique: true,
                filter: "[Street] IS NOT NULL AND [PostalCode] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Addresses_Street_PostalCode_CityDbMCityId",
                schema: "supusr",
                table: "Addresses");
        }
    }
}
