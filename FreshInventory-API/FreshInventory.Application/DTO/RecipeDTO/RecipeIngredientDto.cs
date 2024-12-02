namespace FreshInventory.Application.DTO.RecipeDTO
{
    public class RecipeIngredientDto
    {
        public int IngredientId { get; set; }
        public int? RecipeId { get; set; }
        public int Quantity { get; set; }
    }
}
