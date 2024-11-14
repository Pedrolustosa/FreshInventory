using FreshInventory.Application.Common;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.Interfaces
{
    public interface IRecipeService
    {
        Task<PagedList<RecipeDto>> GetAllRecipesAsync(int pageNumber, int pageSize, string? name = null, string? sortBy = null, string? sortDirection = null);
        Task<RecipeDto> CreateRecipeAsync(RecipeCreateDto recipeCreateDto);
        Task<RecipeDto> UpdateRecipeAsync(RecipeUpdateDto recipeUpdateDto);
        Task DeleteRecipeAsync(int recipeId);
        Task ReactivateRecipeAsync(int recipeId);
        Task<bool> ReserveIngredientsAsync(int recipeId);
        Task<RecipeDto> GetRecipeByIdAsync(int recipeId);
    }
}
