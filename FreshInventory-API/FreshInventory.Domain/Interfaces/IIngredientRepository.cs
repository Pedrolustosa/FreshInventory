using FreshInventory.Domain.Common.Models;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Domain.Interfaces
{
    public interface IIngredientRepository
    {
        Task<Ingredient> GetIngredientByIdAsync(int ingredientId);
        Task<PaginatedList<Ingredient>> GetAllIngredientsPagedAsync(int pageNumber, int pageSize);
        Task AddIngredientAsync(Ingredient ingredient);
        Task UpdateIngredientAsync(Ingredient ingredient);
        Task<bool> DeleteIngredientAsync(int ingredientId);
    }
}
