using FreshInventory.Domain.Entities;

namespace FreshInventory.Domain.Interfaces;

public interface IIngredientRepository
{
    Task AddAsync(Ingredient ingredient);
    Task UpdateAsync(Ingredient ingredient);
    Task DeleteAsync(int id);
    Task<Ingredient> GetByIdAsync(int id);
    Task<(IEnumerable<Ingredient> Items, int TotalCount)> GetAllIngredientsAsync(
            int pageNumber,
            int pageSize,
            string? name = null,
            string? category = null,
            string? sortBy = null,
            string? sortDirection = null);
}
