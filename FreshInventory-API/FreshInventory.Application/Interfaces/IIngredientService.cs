using FreshInventory.Application.Common;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.Interfaces;

public interface IIngredientService
{
    Task<PagedList<IngredientDto>> GetAllIngredientsAsync(
        int pageNumber, int pageSize, string? name = null, string? category = null, string? sortBy = null, string? sortDirection = null);

    Task<IngredientDto> GetIngredientByIdAsync(int id);

    Task<IngredientDto> AddIngredientAsync(IngredientCreateDto ingredientCreateDto);

    Task UpdateIngredientAsync(IngredientUpdateDto ingredientUpdateDto);

    Task DeleteIngredientAsync(int id);
}
