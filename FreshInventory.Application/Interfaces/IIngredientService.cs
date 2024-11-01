using FreshInventory.Application.DTO;

namespace FreshInventory.Application.Interfaces;

public interface IIngredientService
{
    Task AddIngredientAsync(IngredientCreateDto ingredientCreateDto);
    Task UpdateIngredientAsync(IngredientUpdateDto ingredientUpdateDto);
    Task DeleteIngredientAsync(int id);
    Task<IngredientDto> GetIngredientByIdAsync(int id);
    Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync();
}
