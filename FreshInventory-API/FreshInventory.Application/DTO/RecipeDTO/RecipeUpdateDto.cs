namespace FreshInventory.Application.DTO.RecipeDTO
{
    public record RecipeUpdateDto(int Id, string Name, List<RecipeIngredientDto> Ingredients);
}