using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.Application.Services;

public class IngredientService(IIngredientRepository repository, IMapper mapper, ILogger<IngredientService> logger) : IIngredientService
{
    private readonly IIngredientRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IngredientService> _logger = logger;

    public async Task AddIngredientAsync(IngredientCreateDto ingredientCreateDto)
    {
        try
        {
            var ingredient = _mapper.Map<Ingredient>(ingredientCreateDto);
            await _repository.AddAsync(ingredient);
            _logger.LogInformation("Ingredient '{Name}' added successfully.", ingredient.Name);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Failed to add ingredient '{Name}'.", ingredientCreateDto.Name);
            throw new ServiceException("Failed to add ingredient.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while adding ingredient '{Name}'.", ingredientCreateDto.Name);
            throw new ServiceException("An unexpected error occurred while adding the ingredient.", ex);
        }
    }

    public async Task UpdateIngredientAsync(IngredientUpdateDto ingredientUpdateDto)
    {
        try
        {
            var ingredient = _mapper.Map<Ingredient>(ingredientUpdateDto);
            await _repository.UpdateAsync(ingredient);
            _logger.LogInformation("Ingredient '{Name}' updated successfully.", ingredient.Name);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Failed to update ingredient '{Name}'.", ingredientUpdateDto.Name);
            throw new ServiceException("Failed to update ingredient.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating ingredient '{Name}'.", ingredientUpdateDto.Name);
            throw new ServiceException("An unexpected error occurred while updating the ingredient.", ex);
        }
    }

    public async Task DeleteIngredientAsync(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
            _logger.LogInformation("Ingredient with ID {Id} deleted successfully.", id);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Failed to delete ingredient with ID {Id}.", id);
            throw new ServiceException("Failed to delete ingredient.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting ingredient with ID {Id}.", id);
            throw new ServiceException("An unexpected error occurred while deleting the ingredient.", ex);
        }
    }

    public async Task<IngredientDto> GetIngredientByIdAsync(int id)
    {
        try
        {
            var ingredient = await _repository.GetByIdAsync(id);
            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);
            _logger.LogInformation("Ingredient with ID {Id} retrieved successfully.", id);
            return ingredientDto;
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Failed to retrieve ingredient with ID {Id}.", id);
            throw new ServiceException("Failed to retrieve ingredient.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving ingredient with ID {Id}.", id);
            throw new ServiceException("An unexpected error occurred while retrieving the ingredient.", ex);
        }
    }

    public async Task<IEnumerable<IngredientDto>> GetAllIngredientsAsync()
    {
        try
        {
            var ingredients = await _repository.GetAllAsync();
            var ingredientDtos = _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
            _logger.LogInformation("All ingredients retrieved successfully.");
            return ingredientDtos;
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Failed to retrieve ingredients.");
            throw new ServiceException("Failed to retrieve ingredients.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving ingredients.");
            throw new ServiceException("An unexpected error occurred while retrieving ingredients.", ex);
        }
    }
}
