namespace FreshInventory.Application.DTO.RecipeDTO
{
    public class RecipeCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
        public List<string> Instructions { get; set; }
    }
}
