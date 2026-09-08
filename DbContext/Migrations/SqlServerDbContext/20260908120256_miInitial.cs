using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbContext.Migrations.SqlServerDbContext
{
    /// <inheritdoc />
    public partial class miInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "supusr");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "supusr",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "supusr",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "supusr",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "varchar(200)", nullable: true),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "supusr",
                columns: table => new
                {
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryDbMCountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityName = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryDbMCountryId",
                        column: x => x.CountryDbMCountryId,
                        principalSchema: "supusr",
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                schema: "supusr",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityDbMCityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street = table.Column<string>(type: "varchar(200)", nullable: true),
                    PostalCode = table.Column<string>(type: "varchar(200)", nullable: true),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_CityDbMCityId",
                        column: x => x.CityDbMCityId,
                        principalSchema: "supusr",
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attractions",
                schema: "supusr",
                columns: table => new
                {
                    AttractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressDbMAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AttractionName = table.Column<string>(type: "varchar(200)", nullable: true),
                    AttractionDescription = table.Column<string>(type: "varchar(200)", nullable: true),
                    Seeded = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attractions", x => x.AttractionId);
                    table.ForeignKey(
                        name: "FK_Attractions_Addresses_AddressDbMAddressId",
                        column: x => x.AddressDbMAddressId,
                        principalSchema: "supusr",
                        principalTable: "Addresses",
                        principalColumn: "AddressId");
                });

            migrationBuilder.CreateTable(
                name: "AttractionDbMCategoryDbM",
                schema: "supusr",
                columns: table => new
                {
                    AttractionDbMAttractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryDbMCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttractionDbMCategoryDbM", x => new { x.AttractionDbMAttractionId, x.CategoryDbMCategoryId });
                    table.ForeignKey(
                        name: "FK_AttractionDbMCategoryDbM_Attractions_AttractionDbMAttractionId",
                        column: x => x.AttractionDbMAttractionId,
                        principalSchema: "supusr",
                        principalTable: "Attractions",
                        principalColumn: "AttractionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttractionDbMCategoryDbM_Categories_CategoryDbMCategoryId",
                        column: x => x.CategoryDbMCategoryId,
                        principalSchema: "supusr",
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                schema: "supusr",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttractionDbMAttractionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserDbMUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommentText = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Attractions_AttractionDbMAttractionId",
                        column: x => x.AttractionDbMAttractionId,
                        principalSchema: "supusr",
                        principalTable: "Attractions",
                        principalColumn: "AttractionId");
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserDbMUserId",
                        column: x => x.UserDbMUserId,
                        principalSchema: "supusr",
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityDbMCityId",
                schema: "supusr",
                table: "Addresses",
                column: "CityDbMCityId");

            migrationBuilder.CreateIndex(
                name: "IX_AttractionDbMCategoryDbM_CategoryDbMCategoryId",
                schema: "supusr",
                table: "AttractionDbMCategoryDbM",
                column: "CategoryDbMCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Attractions_AddressDbMAddressId",
                schema: "supusr",
                table: "Attractions",
                column: "AddressDbMAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryDbMCountryId",
                schema: "supusr",
                table: "Cities",
                column: "CountryDbMCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AttractionDbMAttractionId",
                schema: "supusr",
                table: "Comments",
                column: "AttractionDbMAttractionId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserDbMUserId",
                schema: "supusr",
                table: "Comments",
                column: "UserDbMUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "supusr",
                table: "Users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttractionDbMCategoryDbM",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Comments",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Attractions",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Addresses",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "supusr");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "supusr");
        }
    }
}
