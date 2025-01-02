using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshInventory.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnSupplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngredientName",
                table: "RecipeIngredients");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Suppliers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Suppliers");

            migrationBuilder.AddColumn<string>(
                name: "IngredientName",
                table: "RecipeIngredients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
