using FreshInventory.Application.DTO.RecipeDTO;

namespace FreshInventory.Application.Interfaces
{
    public interface IRecipeService
    {
        Task<RecipeReadDto> CreateRecipeAsync(RecipeCreateDto recipeCreateDto);
        Task<RecipeReadDto> UpdateRecipeAsync(int recipeId, RecipeUpdateDto recipeUpdateDto);
        Task<bool> DeleteRecipeAsync(int recipeId);
        Task<RecipeReadDto> GetRecipeByIdAsync(int recipeId);
        Task<IEnumerable<RecipeReadDto>> GetAllRecipesAsync();
    }
}
