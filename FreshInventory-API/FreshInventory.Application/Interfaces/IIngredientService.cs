using FreshInventory.Application.DTO.IngredientDTO;

namespace FreshInventory.Application.Interfaces
{
    public interface IIngredientService
    {
        Task<IngredientReadDto> CreateIngredientAsync(IngredientCreateDto ingredientCreateDto);
        Task<IEnumerable<IngredientReadDto>> GetAllIngredientsAsync();
        Task<IngredientReadDto> GetIngredientByIdAsync(int ingredientId);
        Task<IngredientReadDto> UpdateIngredientAsync(int ingredientId, IngredientUpdateDto ingredientUpdateDto);
        Task<bool> DeleteIngredientAsync(int ingredientId);
    }
}
