using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Common.Models;

namespace FreshInventory.Application.Interfaces
{
    public interface IIngredientService
    {
        Task<IngredientReadDto> CreateIngredientAsync(IngredientCreateDto ingredientCreateDto);
        Task<PaginatedList<IngredientReadDto>> GetAllIngredientsPagedAsync(int pageNumber, int pageSize);
        Task<IngredientReadDto> GetIngredientByIdAsync(int ingredientId);
        Task<IngredientReadDto> UpdateIngredientAsync(int ingredientId, IngredientUpdateDto ingredientUpdateDto);
        Task<bool> DeleteIngredientAsync(int ingredientId);
    }
}
