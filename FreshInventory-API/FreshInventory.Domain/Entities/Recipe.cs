namespace FreshInventory.Domain.Entities
{
    public class Recipe
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Category { get; private set; }
        public string PreparationTime { get; private set; }
        public string Servings { get; private set; }
        public List<string> Instructions { get; private set; } = new List<string>();
        public bool IsAvailable { get; private set; }
        public bool IsDeleted { get; private set; } = false;
        public DateTime CreatedDate { get; private set; }
        public DateTime UpdatedDate { get; private set; }
        public ICollection<RecipeIngredient> Ingredients { get; private set; } = new List<RecipeIngredient>();

        public Recipe() { }

        public void Update(string name, string description, string category, string preparationTime, string servings, bool isAvailable, List<RecipeIngredient> ingredients, List<string> instructions)
        {
            Name = name;
            Description = description;
            Category = category;
            PreparationTime = preparationTime;
            Servings = servings;
            IsAvailable = isAvailable;
            Ingredients = ingredients ?? [];
            Instructions = instructions ?? [];
            UpdatedDate = DateTime.UtcNow;
        }
    }
}