using FreshInventory.Domain.Common.Models;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Domain.Interfaces
{
    public interface IRecipeRepository
    {
        Task<Recipe> GetRecipeByIdAsync(int recipeId);
        Task<PaginatedList<Recipe>> GetAllRecipesPagedAsync(int pageNumber, int pageSize);
        Task AddRecipeAsync(Recipe recipe);
        Task UpdateRecipeAsync(Recipe recipe);
        Task<bool> DeleteRecipeAsync(int recipeId);
    }
}
