using FreshInventory.Application.DTO;

namespace FreshInventory.Application.Interfaces;

public interface IIngredientService
{
    Task AddIngredientAsync(IngredientDto ingredientDto);
    Task UpdateIngredientAsync(IngredientDto ingredientDto);
    Task DeleteIngredientAsync(int id);
    Task<IngredientDto> GetIngredientByIdAsync(int id);
    Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync();
}
