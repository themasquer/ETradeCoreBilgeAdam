using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ETradeCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETradeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ETradeCountries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETradeCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ETradeRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETradeRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ETradeProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UnitPrice = table.Column<double>(type: "float", nullable: false),
                    StockAmount = table.Column<int>(type: "int", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    ImageFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETradeProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ETradeProducts_ETradeCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ETradeCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ETradeCities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETradeCities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ETradeCities_ETradeCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "ETradeCountries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ETradeUserDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETradeUserDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ETradeUserDetails_ETradeCities_CityId",
                        column: x => x.CityId,
                        principalTable: "ETradeCities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ETradeUserDetails_ETradeCountries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "ETradeCountries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ETradeUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    UserDetailId = table.Column<int>(type: "int", nullable: false),
                    Guid = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ETradeUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ETradeUsers_ETradeRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "ETradeRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ETradeUsers_ETradeUserDetails_UserDetailId",
                        column: x => x.UserDetailId,
                        principalTable: "ETradeUserDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ETradeCities_CountryId",
                table: "ETradeCities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ETradeProducts_CategoryId",
                table: "ETradeProducts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ETradeProducts_Name",
                table: "ETradeProducts",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ETradeUserDetails_CityId",
                table: "ETradeUserDetails",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ETradeUserDetails_CountryId",
                table: "ETradeUserDetails",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ETradeUserDetails_EMail",
                table: "ETradeUserDetails",
                column: "EMail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ETradeUsers_RoleId",
                table: "ETradeUsers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ETradeUsers_UserDetailId",
                table: "ETradeUsers",
                column: "UserDetailId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ETradeProducts");

            migrationBuilder.DropTable(
                name: "ETradeUsers");

            migrationBuilder.DropTable(
                name: "ETradeCategories");

            migrationBuilder.DropTable(
                name: "ETradeRoles");

            migrationBuilder.DropTable(
                name: "ETradeUserDetails");

            migrationBuilder.DropTable(
                name: "ETradeCities");

            migrationBuilder.DropTable(
                name: "ETradeCountries");
        }
    }
}
