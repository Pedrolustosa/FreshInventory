using FreshInventory.Domain.Entities;

namespace FreshInventory.Application.Interfaces;

public interface IIngredientService
{
    Task AddIngredientAsync(Ingredient ingredient);

    Task UpdateIngredientAsync(Ingredient ingredient);

    Task DeleteIngredientAsync(int id);

    Task<Ingredient> GetIngredientByIdAsync(int id);

    Task<IEnumerable<Ingredient>> GetAllIngredientsAsync();
}
