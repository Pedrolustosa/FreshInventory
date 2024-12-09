namespace FreshInventory.Domain.Entities
{
    public class Recipe : EntityBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Servings { get; private set; }
        public TimeSpan PreparationTime { get; private set; }
        public List<RecipeIngredient> RecipeIngredients { get; private set; } = new();
        public List<string> Steps { get; private set; } = new();

        private Recipe() { }

        public Recipe(
            string name,
            string description,
            int servings,
            TimeSpan preparationTime,
            List<RecipeIngredient> recipeIngredients,
            List<string> steps)
        {
            SetName(name);
            SetDescription(description);
            SetServings(servings);
            SetPreparationTime(preparationTime);
            SetRecipeIngredients(recipeIngredients);
            SetSteps(steps);
            SetCreatedDate();
        }

        public void UpdateRecipe(
            string name,
            string description,
            int servings,
            TimeSpan preparationTime,
            List<RecipeIngredient> recipeIngredients,
            List<string> steps)
        {
            SetName(name);
            SetDescription(description);
            SetServings(servings);
            SetPreparationTime(preparationTime);
            SetRecipeIngredients(recipeIngredients);
            SetSteps(steps);
            UpdateTimestamp();
        }

        private void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");
            Name = name;
        }

        private void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be null or empty.");
            Description = description;
        }

        private void SetServings(int servings)
        {
            if (servings <= 0)
                throw new ArgumentException("Servings must be greater than zero.");
            Servings = servings;
        }

        private void SetPreparationTime(TimeSpan preparationTime)
        {
            if (preparationTime <= TimeSpan.Zero)
                throw new ArgumentException("Preparation time must be greater than zero.");
            PreparationTime = preparationTime;
        }

        private void SetSteps(List<string> steps)
        {
            if (steps == null || steps.Count == 0)
                throw new ArgumentException("A recipe must have at least one step.");
            Steps = steps;
        }

        private void SetRecipeIngredients(List<RecipeIngredient> recipeIngredients)
        {
            if (recipeIngredients == null || recipeIngredients.Count == 0)
                throw new ArgumentException("A recipe must have at least one ingredient.");
            RecipeIngredients = recipeIngredients;
        }
    }
}
