namespace FreshInventory.Domain.Entities
{
    public class RecipeIngredient
    {
        public int RecipeId { get; private set; }
        public int IngredientId { get; private set; }
        public int QuantityRequired { get; private set; }

        public virtual Recipe Recipe { get; private set; }
        public virtual Ingredient Ingredient { get; private set; }

        private RecipeIngredient() { }

        public RecipeIngredient(int ingredientId, int quantityRequired)
        {
            if (ingredientId <= 0) throw new ArgumentException("Ingredient ID must be valid.", nameof(ingredientId));
            if (quantityRequired <= 0) throw new ArgumentException("Quantity required must be greater than zero.", nameof(quantityRequired));

            IngredientId = ingredientId;
            QuantityRequired = quantityRequired;
        }

        // Existing constructor with RecipeId
        public RecipeIngredient(int recipeId, int ingredientId, int quantityRequired)
        {
            if (recipeId <= 0) throw new ArgumentException("Recipe ID must be valid.", nameof(recipeId));
            if (ingredientId <= 0) throw new ArgumentException("Ingredient ID must be valid.", nameof(ingredientId));
            if (quantityRequired <= 0) throw new ArgumentException("Quantity required must be greater than zero.", nameof(quantityRequired));

            RecipeId = recipeId;
            IngredientId = ingredientId;
            QuantityRequired = quantityRequired;
        }

        public void UpdateQuantity(int newQuantity)
        {
            if (newQuantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(newQuantity));
            QuantityRequired = newQuantity;
        }
    }
}
