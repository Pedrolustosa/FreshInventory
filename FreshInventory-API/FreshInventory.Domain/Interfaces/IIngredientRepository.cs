using FreshInventory.Domain.Entities;

namespace FreshInventory.Domain.Interfaces
{
    public interface IIngredientRepository
    {
        Task<Ingredient> GetIngredientByIdAsync(int ingredientId);
        Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
        Task AddIngredientAsync(Ingredient ingredient);
        Task UpdateIngredientAsync(Ingredient ingredient);
        Task<bool> DeleteIngredientAsync(int ingredientId);
    }
}
