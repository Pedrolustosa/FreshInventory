namespace FreshInventory.Domain.Entities
{
    public class RecipeIngredient
    {
        public int Id { get; private set; }
        public int RecipeId { get; private set; }
        public Recipe Recipe { get; private set; }
        public int IngredientId { get; private set; }
        public Ingredient Ingredient { get; private set; }
        public int QuantityRequired { get; private set; }

        private RecipeIngredient() { }

        public RecipeIngredient(int ingredientId, int quantityRequired)
        {
            IngredientId = ingredientId;
            QuantityRequired = quantityRequired;
        }

        public void SetQuantityRequired(int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity required must be greater than zero.");
            QuantityRequired = quantity;
        }
    }
}