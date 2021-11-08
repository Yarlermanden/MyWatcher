using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWatcherApi.Migrations
{
    public partial class UpdatedItemPriceThisWeekName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceThisMonday",
                table: "Item",
                newName: "PriceThisWeek");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceThisWeek",
                table: "Item",
                newName: "PriceThisMonday");
        }
    }
}
