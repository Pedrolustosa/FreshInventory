using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.Application.Service;

public class IngredientService(IIngredientRepository repository) : IIngredientService
{
    private readonly IIngredientRepository _repository = repository;

    public async Task AddIngredientAsync(Ingredient ingredient)
    {
        await _repository.AddAsync(ingredient);
    }

    public async Task UpdateIngredientAsync(Ingredient ingredient)
    {
        await _repository.UpdateAsync(ingredient);
    }

    public async Task DeleteIngredientAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<Ingredient> GetIngredientByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
    {
        return await _repository.GetAllAsync();
    }
}
