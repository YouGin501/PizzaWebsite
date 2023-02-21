using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PizzaWebsite.Migrations
{
    public partial class CategoryFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_PizzaCategory_CategoryId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Product",
                newName: "PizzaCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryId",
                table: "Product",
                newName: "IX_Product_PizzaCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_PizzaCategory_PizzaCategoryId",
                table: "Product",
                column: "PizzaCategoryId",
                principalTable: "PizzaCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_PizzaCategory_PizzaCategoryId",
                table: "Product");

            migrationBuilder.RenameColumn(
                name: "PizzaCategoryId",
                table: "Product",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_PizzaCategoryId",
                table: "Product",
                newName: "IX_Product_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_PizzaCategory_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "PizzaCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
