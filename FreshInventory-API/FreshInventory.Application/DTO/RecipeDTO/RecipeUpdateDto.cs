namespace FreshInventory.Application.DTO.RecipeDTO
{
    public class RecipeUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string PreparationTime { get; set; }
        public string Servings { get; set; }
        public List<string> Instructions { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
    }
}