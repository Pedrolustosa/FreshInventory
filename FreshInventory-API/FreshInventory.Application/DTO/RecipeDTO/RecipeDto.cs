namespace FreshInventory.Application.DTO.RecipeDTO
{
    public record RecipeDto(int Id, string Name, bool IsActive, List<RecipeIngredientDto> Ingredients);
}