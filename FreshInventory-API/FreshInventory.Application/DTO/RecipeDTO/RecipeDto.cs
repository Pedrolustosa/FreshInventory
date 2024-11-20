namespace FreshInventory.Application.DTO.RecipeDTO
{
    public class RecipeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string PreparationTime { get; set; }
        public string Servings { get; set; }
        public List<string> Instructions { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
    }
}