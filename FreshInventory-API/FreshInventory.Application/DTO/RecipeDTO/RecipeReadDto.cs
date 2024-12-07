namespace FreshInventory.Application.DTO.RecipeDTO
{
    public class RecipeReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<RecipeIngredientDto> Ingredients { get; set; }
        public List<string> Steps { get; set; } = new List<string>();
    }
}
