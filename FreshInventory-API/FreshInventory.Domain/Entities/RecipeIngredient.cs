namespace FreshInventory.Domain.Entities
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public int Quantity { get; set; }

        public Ingredient Ingredient { get; set; }  

        // Construtor vazio para EF Core
        public RecipeIngredient() { }

        public RecipeIngredient(int ingredientId, int quantity)
        {
            IngredientId = ingredientId;
            Quantity = quantity;
        }
    }
}