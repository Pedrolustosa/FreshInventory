using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.Application.Services;

public class IngredientService(IIngredientRepository repository, IMapper mapper) : IIngredientService
{
    private readonly IMapper _mapper = mapper;
    private readonly IIngredientRepository _repository = repository;

    public async Task AddIngredientAsync(IngredientDto ingredientDto)
    {
        var ingredient = _mapper.Map<Ingredient>(ingredientDto);
        await _repository.AddAsync(ingredient);
    }

    public async Task UpdateIngredientAsync(IngredientDto ingredientDto)
    {
        var ingredient = _mapper.Map<Ingredient>(ingredientDto);
        await _repository.UpdateAsync(ingredient);
    }

    public async Task DeleteIngredientAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IngredientDto> GetIngredientByIdAsync(int id)
    {
        var ingredient = await _repository.GetByIdAsync(id);
        return _mapper.Map<IngredientDto>(ingredient);
    }

    public async Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync()
    {
        var ingredients = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
    }
}
