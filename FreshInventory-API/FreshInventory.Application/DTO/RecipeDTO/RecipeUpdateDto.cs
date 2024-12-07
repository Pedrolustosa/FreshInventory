namespace FreshInventory.Application.DTO.RecipeDTO
{
    public class RecipeUpdateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Servings { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
        public List<string> Steps { get; set; } = new List<string>();
    }

}
