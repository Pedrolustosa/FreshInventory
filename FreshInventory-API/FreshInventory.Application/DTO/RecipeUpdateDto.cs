namespace FreshInventory.Application.DTO
{
    public record RecipeUpdateDto(int Id, string Name, List<RecipeIngredientDto> Ingredients);
}