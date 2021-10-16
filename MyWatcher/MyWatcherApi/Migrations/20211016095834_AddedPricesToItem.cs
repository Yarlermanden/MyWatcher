using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWatcherApi.Migrations
{
    public partial class AddedPricesToItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastWeeklyPriceUpdate",
                table: "Item",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PriceLastWeek",
                table: "Item",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PriceLowestKnown",
                table: "Item",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PriceThisMonday",
                table: "Item",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastWeeklyPriceUpdate",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "PriceLastWeek",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "PriceLowestKnown",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "PriceThisMonday",
                table: "Item");
        }
    }
}
