namespace FreshInventory.Domain.Entities;

public class Recipe
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Instructions { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime UpdatedDate { get; private set; }
    private readonly List<IngredientRecipe> _ingredientRecipes = new();

    public IReadOnlyCollection<IngredientRecipe> IngredientRecipes => _ingredientRecipes.AsReadOnly();

    public Recipe(string name, string instructions)
    {
        SetName(name);
        SetInstructions(instructions);
        CreatedDate = DateTime.Now;
        UpdatedDate = DateTime.Now;
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Recipe name cannot be null or empty.");

        Name = name;
        UpdatedDate = DateTime.Now;
    }

    public void SetInstructions(string instructions)
    {
        if (string.IsNullOrWhiteSpace(instructions))
            throw new ArgumentException("Instructions cannot be null or empty.");

        Instructions = instructions;
        UpdatedDate = DateTime.Now;
    }

    public void AddIngredient(Ingredient ingredient, int quantity)
    {
        if (ingredient == null) throw new ArgumentNullException(nameof(ingredient));
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.");

        var existingIngredient = _ingredientRecipes.FirstOrDefault(ir => ir.IngredientId == ingredient.Id);
        if (existingIngredient != null)
        {
            existingIngredient.IncreaseQuantity(quantity);
        }
        else
        {
            _ingredientRecipes.Add(new IngredientRecipe(Id, ingredient.Id, quantity));
        }

        UpdatedDate = DateTime.Now;
    }

    public void RemoveIngredient(int ingredientId)
    {
        var ingredientRecipe = _ingredientRecipes.FirstOrDefault(ir => ir.IngredientId == ingredientId);
        if (ingredientRecipe != null)
        {
            _ingredientRecipes.Remove(ingredientRecipe);
            UpdatedDate = DateTime.Now;
        }
    }

    public bool HasSufficientIngredients(IReadOnlyCollection<Ingredient> availableIngredients)
    {
        return _ingredientRecipes.All(ir =>
            availableIngredients.Any(ai => ai.Id == ir.IngredientId && ai.Quantity >= ir.Quantity));
    }

    public void ReserveIngredients(IReadOnlyCollection<Ingredient> availableIngredients)
    {
        if (!HasSufficientIngredients(availableIngredients))
            throw new InvalidOperationException("Insufficient ingredients for this recipe.");

        foreach (var ingredientRecipe in _ingredientRecipes)
        {
            var ingredient = availableIngredients.First(ai => ai.Id == ingredientRecipe.IngredientId);
            ingredient.ReduceQuantity(ingredientRecipe.Quantity);
        }
    }
}
