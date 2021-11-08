using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWatcherApi.Migrations
{
    public partial class added_country_and_continent_to_useritem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Continent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Continent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SecondHandItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Service = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    PriceLastWeek = table.Column<double>(type: "double precision", nullable: false),
                    PriceThisWeek = table.Column<double>(type: "double precision", nullable: false),
                    PriceLowestKnown = table.Column<double>(type: "double precision", nullable: false),
                    LastWeeklyPriceUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    URL = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    LastScan = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondHandItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Service = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    PriceLastWeek = table.Column<double>(type: "double precision", nullable: false),
                    PriceThisWeek = table.Column<double>(type: "double precision", nullable: false),
                    PriceLowestKnown = table.Column<double>(type: "double precision", nullable: false),
                    LastWeeklyPriceUpdate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    URL = table.Column<string>(type: "text", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    LastScan = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockItem", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Salt = table.Column<string>(type: "text", nullable: false),
                    LastLogin = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CountryCode = table.Column<string>(type: "text", nullable: false),
                    ContinentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Country_Continent_ContinentId",
                        column: x => x.ContinentId,
                        principalTable: "Continent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSecondHandItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SecondHandItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContinentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSecondHandItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSecondHandItem_Continent_ContinentId",
                        column: x => x.ContinentId,
                        principalTable: "Continent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserSecondHandItem_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserSecondHandItem_SecondHandItem_SecondHandItemId",
                        column: x => x.SecondHandItemId,
                        principalTable: "SecondHandItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSecondHandItem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStockItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StockItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContinentId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStockItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStockItem_Continent_ContinentId",
                        column: x => x.ContinentId,
                        principalTable: "Continent",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserStockItem_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserStockItem_StockItem_StockItemId",
                        column: x => x.StockItemId,
                        principalTable: "StockItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStockItem_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Website",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    BaseUrl = table.Column<string>(type: "text", nullable: false),
                    Regexes = table.Column<string>(type: "text", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Website", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Website_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WebsiteSecondHandItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SecondHandItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    WebsiteId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteSecondHandItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsiteSecondHandItem_SecondHandItem_SecondHandItemId",
                        column: x => x.SecondHandItemId,
                        principalTable: "SecondHandItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebsiteSecondHandItem_Website_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Website",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WebsiteStockItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StockItemId = table.Column<Guid>(type: "uuid", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    WebsiteId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebsiteStockItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebsiteStockItem_StockItem_StockItemId",
                        column: x => x.StockItemId,
                        principalTable: "StockItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WebsiteStockItem_Website_WebsiteId",
                        column: x => x.WebsiteId,
                        principalTable: "Website",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Country_ContinentId",
                table: "Country",
                column: "ContinentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecondHandItem_ContinentId",
                table: "UserSecondHandItem",
                column: "ContinentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecondHandItem_CountryId",
                table: "UserSecondHandItem",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecondHandItem_SecondHandItemId",
                table: "UserSecondHandItem",
                column: "SecondHandItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSecondHandItem_UserId",
                table: "UserSecondHandItem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStockItem_ContinentId",
                table: "UserStockItem",
                column: "ContinentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStockItem_CountryId",
                table: "UserStockItem",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStockItem_StockItemId",
                table: "UserStockItem",
                column: "StockItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStockItem_UserId",
                table: "UserStockItem",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Website_CountryId",
                table: "Website",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteSecondHandItem_SecondHandItemId",
                table: "WebsiteSecondHandItem",
                column: "SecondHandItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteSecondHandItem_WebsiteId",
                table: "WebsiteSecondHandItem",
                column: "WebsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteStockItem_StockItemId",
                table: "WebsiteStockItem",
                column: "StockItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WebsiteStockItem_WebsiteId",
                table: "WebsiteStockItem",
                column: "WebsiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSecondHandItem");

            migrationBuilder.DropTable(
                name: "UserStockItem");

            migrationBuilder.DropTable(
                name: "WebsiteSecondHandItem");

            migrationBuilder.DropTable(
                name: "WebsiteStockItem");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "SecondHandItem");

            migrationBuilder.DropTable(
                name: "StockItem");

            migrationBuilder.DropTable(
                name: "Website");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Continent");
        }
    }
}
