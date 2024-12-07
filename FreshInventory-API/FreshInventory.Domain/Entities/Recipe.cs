namespace FreshInventory.Domain.Entities
{
    public class Recipe : EntityBase
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Servings { get; private set; }
        public TimeSpan PreparationTime { get; private set; }
        public Dictionary<int, int> Ingredients { get; private set; } = new();
        public List<string> Steps { get; private set; } = new();

        private Recipe() { }

        public Recipe(
            string name,
            string description,
            int servings,
            TimeSpan preparationTime,
            Dictionary<int, int> ingredients,
            List<string> steps)
        {
            SetName(name);
            SetDescription(description);
            SetServings(servings);
            SetPreparationTime(preparationTime);
            SetIngredients(ingredients);
            SetSteps(steps);

            SetCreatedDate();
        }

        public void UpdateRecipe(
            string name,
            string description,
            int servings,
            TimeSpan preparationTime,
            Dictionary<int, int> ingredients,
            List<string> steps)
        {
            SetName(name);
            SetDescription(description);
            SetServings(servings);
            SetPreparationTime(preparationTime);
            SetIngredients(ingredients);
            SetSteps(steps);

            UpdateTimestamp(); // Atualiza UpdatedDate para refletir a modificação.
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");
            Name = name;
        }

        public void SetDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be null or empty.");
            Description = description;
        }

        public void SetServings(int servings)
        {
            if (servings <= 0)
                throw new ArgumentException("Servings must be greater than zero.");
            Servings = servings;
        }

        public void SetPreparationTime(TimeSpan preparationTime)
        {
            if (preparationTime <= TimeSpan.Zero)
                throw new ArgumentException("Preparation time must be greater than zero.");
            PreparationTime = preparationTime;
        }

        public void SetIngredients(Dictionary<int, int> ingredients)
        {
            if (ingredients == null || ingredients.Count == 0)
                throw new ArgumentException("A recipe must have at least one ingredient.");
            Ingredients = ingredients;
        }

        public void SetSteps(List<string> steps)
        {
            if (steps == null || steps.Count == 0)
                throw new ArgumentException("A recipe must have at least one step.");
            Steps = steps;
        }
    }
}
