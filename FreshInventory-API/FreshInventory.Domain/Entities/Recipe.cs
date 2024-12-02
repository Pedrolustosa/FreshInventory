namespace FreshInventory.Domain.Entities
{
    public class Recipe
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Servings { get; private set; }
        public TimeSpan PreparationTime { get; private set; }
        public List<RecipeIngredient> Ingredients { get; private set; } = new List<RecipeIngredient>();
        public List<string> Instructions { get; private set; } = new List<string>();
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }

        private Recipe() { }

        public Recipe(
            string name,
            string description,
            int servings,
            TimeSpan preparationTime,
            List<RecipeIngredient> ingredients,
            List<string> instructions)
        {
            SetName(name);
            SetDescription(description);
            SetServings(servings);
            SetPreparationTime(preparationTime);
            SetIngredients(ingredients);
            SetInstructions(instructions);
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Recipe name cannot be null or empty.");
            Name = name;
            UpdateTimestamp();
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("Recipe description cannot be null or empty.");
            Description = description;
            UpdateTimestamp();
        }

        public void SetServings(int servings)
        {
            if (servings <= 0) throw new ArgumentException("Servings must be greater than zero.");
            Servings = servings;
            UpdateTimestamp();
        }

        public void SetPreparationTime(TimeSpan preparationTime)
        {
            if (preparationTime <= TimeSpan.Zero) throw new ArgumentException("Preparation time must be greater than zero.");
            PreparationTime = preparationTime;
            UpdateTimestamp();
        }

        public void SetIngredients(List<RecipeIngredient> ingredients)
        {
            if (ingredients == null || ingredients.Count == 0) throw new ArgumentException("A recipe must have at least one ingredient.");
            Ingredients = ingredients;
            UpdateTimestamp();
        }

        public void SetInstructions(List<string> instructions)
        {
            if (instructions == null || instructions.Count == 0) throw new ArgumentException("A recipe must have at least one instruction step.");
            Instructions = instructions;
            UpdateTimestamp();
        }

        public void ReduceIngredientsStock(Func<int, Ingredient> getIngredientById)
        {
            foreach (var recipeIngredient in Ingredients)
            {
                var ingredient = getIngredientById(recipeIngredient.IngredientId);
                if (ingredient == null) throw new InvalidOperationException($"Ingredient with ID {recipeIngredient.IngredientId} not found.");
                ingredient.ReduceQuantity(recipeIngredient.QuantityRequired);
            }
        }

        private void UpdateTimestamp()
        {
            UpdatedDate = DateTime.UtcNow;
        }
    }
}
