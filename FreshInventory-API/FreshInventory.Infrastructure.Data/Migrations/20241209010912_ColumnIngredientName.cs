using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FreshInventory.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ColumnIngredientName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IngredientName",
                table: "RecipeIngredients",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngredientName",
                table: "RecipeIngredients");
        }
    }
}
