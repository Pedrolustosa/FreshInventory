using FreshInventory.Domain.Entities;

namespace FreshInventory.Domain.Interfaces
{
    public interface IRecipeRepository
    {
        Task<Recipe> GetRecipeByIdAsync(int recipeId);
        Task<List<Recipe>> GetAllRecipesAsync();
        Task AddRecipeAsync(Recipe recipe);
        Task UpdateRecipeAsync(Recipe recipe);
        Task<bool> DeleteRecipeAsync(int recipeId);
    }
}
