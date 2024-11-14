using FreshInventory.Domain.Entities;

namespace FreshInventory.Domain.Interfaces
{
    public interface IRecipeRepository
    {
        Task<Recipe> AddAsync(Recipe recipe);
        Task UpdateAsync(Recipe recipe);
        Task DeleteAsync(int recipeId);
        Task ReactivateAsync(int recipeId);
        Task<Recipe> GetByIdAsync(int recipeId);
        Task<bool> ReserveIngredientsAsync(int recipeId);

        Task<(IEnumerable<Recipe> Recipes, int TotalCount)> GetAllRecipesAsync(
            int pageNumber,
            int pageSize,
            string? name = null,
            string? sortBy = null,
            string? sortDirection = null);
    }
}
