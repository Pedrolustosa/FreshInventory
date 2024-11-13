namespace FreshInventory.Domain.Entities;

public class IngredientRecipe
{
    public int RecipeId { get; private set; }
    public Recipe Recipe { get; private set; }
    public int IngredientId { get; private set; }
    public Ingredient Ingredient { get; private set; }
    public int Quantity { get; private set; }

    private IngredientRecipe() { }

    public IngredientRecipe(int recipeId, int ingredientId, int quantity)
    {
        RecipeId = recipeId;
        IngredientId = ingredientId;
        SetQuantity(quantity);
    }

    public void SetQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        Quantity = quantity;
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Increase quantity must be positive.");

        Quantity += quantity;
    }

    public void DecreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Decrease quantity must be positive.");

        if (Quantity - quantity < 0)
            throw new InvalidOperationException("Insufficient quantity to reduce.");

        Quantity -= quantity;
    }
}
