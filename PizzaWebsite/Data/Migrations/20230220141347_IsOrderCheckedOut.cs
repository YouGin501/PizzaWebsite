using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaWebsite.Migrations
{
    public partial class IsOrderCheckedOut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOrderCheckedOut",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOrderCheckedOut",
                table: "CartItems");
        }
    }
}
