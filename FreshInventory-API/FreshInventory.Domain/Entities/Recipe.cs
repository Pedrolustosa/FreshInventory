namespace FreshInventory.Domain.Entities
{
    public class Recipe
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; } = true;
        public bool IsDeleted { get; private set; } = false;
        public ICollection<RecipeIngredient> Ingredients { get; private set; }

        private Recipe() { }

        public Recipe(string name, List<RecipeIngredient> ingredients)
        {
            SetName(name);
            Ingredients = ingredients;
        }

        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");
            Name = name;
        }

        public void SetActiveStatus(bool status) => IsActive = status;

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }

        public void Reactivate()
        {
            IsDeleted = false;
        }
    }
}