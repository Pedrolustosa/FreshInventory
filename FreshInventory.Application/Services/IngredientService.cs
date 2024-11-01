using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Entities;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;
using FreshInventory.Application.Interfaces;

namespace FreshInventory.Application.Services;

public class IngredientService(
    IIngredientRepository repository,
    IMapper mapper,
    ILogger<IngredientService> logger,
    IEmailService emailService) : IIngredientService
{
    private readonly IIngredientRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<IngredientService> _logger = logger;
    private readonly IEmailService _emailService = emailService;

    public async Task AddIngredientAsync(IngredientCreateDto ingredientCreateDto)
    {
        try
        {
            var ingredient = _mapper.Map<Ingredient>(ingredientCreateDto);
            await _repository.AddAsync(ingredient);
            _logger.LogInformation("Ingredient '{Name}' added successfully.", ingredient.Name);

            var subject = $"New Ingredient Added: {ingredient.Name}";
            var body = GenerateCreateEmailBody(ingredient);
            await _emailService.SendEmailAsync(subject, body);
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
            var existingIngredient = await _repository.GetByIdAsync(ingredientUpdateDto.Id) ?? throw new ServiceException($"Ingredient with ID {ingredientUpdateDto.Id} not found.");
            var oldIngredientData = _mapper.Map<IngredientDto>(existingIngredient);

            var updatedIngredient = _mapper.Map(ingredientUpdateDto, existingIngredient);
            await _repository.UpdateAsync(updatedIngredient);
            _logger.LogInformation("Ingredient '{Name}' updated successfully.", updatedIngredient.Name);

            var subject = $"Ingredient Updated: {updatedIngredient.Name}";
            var body = GenerateUpdateEmailBody(oldIngredientData, ingredientUpdateDto);
            await _emailService.SendEmailAsync(subject, body);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Failed to update ingredient '{Name}'.", ingredientUpdateDto.Name);
            throw new ServiceException("Failed to update ingredient.", ex);
        }
        catch (ServiceException)
        {
            throw;
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
            var ingredient = await _repository.GetByIdAsync(id) ?? throw new ServiceException($"Ingredient with ID {id} not found.");
            await _repository.DeleteAsync(id);
            _logger.LogInformation("Ingredient with ID {Id} deleted successfully.", id);

            var subject = $"Ingredient Deleted: {ingredient.Name}";
            var body = GenerateDeleteEmailBody(ingredient);
            await _emailService.SendEmailAsync(subject, body);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Failed to delete ingredient with ID {Id}.", id);
            throw new ServiceException("Failed to delete ingredient.", ex);
        }
        catch (ServiceException)
        {
            throw;
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

    private static string GenerateCreateEmailBody(Ingredient ingredient)
    {
        return $@"
                <h2>New Ingredient Added</h2>
                <p>The following ingredient has been added:</p>
                <ul>
                    <li><strong>Name:</strong> {ingredient.Name}</li>
                    <li><strong>Quantity:</strong> {ingredient.Quantity} {ingredient.Unit}</li>
                    <li><strong>Unit Cost:</strong> {ingredient.UnitCost:C}</li>
                    <li><strong>Category:</strong> {ingredient.Category}</li>
                    <li><strong>Supplier:</strong> {ingredient.Supplier}</li>
                    <li><strong>Purchase Date:</strong> {ingredient.PurchaseDate:yyyy-MM-dd}</li>
                    <li><strong>Expiry Date:</strong> {ingredient.ExpiryDate:yyyy-MM-dd}</li>
                    <li><strong>Is Perishable:</strong> {ingredient.IsPerishable}</li>
                    <li><strong>Reorder Level:</strong> {ingredient.ReorderLevel}</li>
                </ul>";
    }

    private static string GenerateUpdateEmailBody(IngredientDto oldData, IngredientUpdateDto newData)
    {
        return $@"
                <h2>Ingredient Updated</h2>
                <p>The ingredient <strong>{oldData.Name}</strong> has been updated.</p>
                <h3>Old Data:</h3>
                <ul>
                    <li><strong>Name:</strong> {oldData.Name}</li>
                    <li><strong>Quantity:</strong> {oldData.Quantity} {oldData.Unit}</li>
                    <li><strong>Unit Cost:</strong> {oldData.UnitCost:C}</li>
                    <li><strong>Category:</strong> {oldData.Category}</li>
                    <li><strong>Supplier:</strong> {oldData.Supplier}</li>
                    <li><strong>Purchase Date:</strong> {oldData.PurchaseDate:yyyy-MM-dd}</li>
                    <li><strong>Expiry Date:</strong> {oldData.ExpiryDate:yyyy-MM-dd}</li>
                    <li><strong>Is Perishable:</strong> {oldData.IsPerishable}</li>
                    <li><strong>Reorder Level:</strong> {oldData.ReorderLevel}</li>
                </ul>
                <h3>New Data:</h3>
                <ul>
                    <li><strong>Name:</strong> {newData.Name}</li>
                    <li><strong>Quantity:</strong> {newData.Quantity} {newData.Unit}</li>
                    <li><strong>Unit Cost:</strong> {newData.UnitCost:C}</li>
                    <li><strong>Category:</strong> {newData.Category}</li>
                    <li><strong>Supplier:</strong> {newData.Supplier}</li>
                    <li><strong>Purchase Date:</strong> {newData.PurchaseDate:yyyy-MM-dd}</li>
                    <li><strong>Expiry Date:</strong> {newData.ExpiryDate:yyyy-MM-dd}</li>
                    <li><strong>Is Perishable:</strong> {newData.IsPerishable}</li>
                    <li><strong>Reorder Level:</strong> {newData.ReorderLevel}</li>
                </ul>";
    }

    private static string GenerateDeleteEmailBody(Ingredient ingredient)
    {
        return $@"
                <h2>Ingredient Deleted</h2>
                <p>The following ingredient has been deleted:</p>
                <ul>
                    <li><strong>Name:</strong> {ingredient.Name}</li>
                    <li><strong>Quantity:</strong> {ingredient.Quantity} {ingredient.Unit}</li>
                    <li><strong>Unit Cost:</strong> {ingredient.UnitCost:C}</li>
                    <li><strong>Category:</strong> {ingredient.Category}</li>
                    <li><strong>Supplier:</strong> {ingredient.Supplier}</li>
                    <li><strong>Purchase Date:</strong> {ingredient.PurchaseDate:yyyy-MM-dd}</li>
                    <li><strong>Expiry Date:</strong> {ingredient.ExpiryDate:yyyy-MM-dd}</li>
                    <li><strong>Is Perishable:</strong> {ingredient.IsPerishable}</li>
                    <li><strong>Reorder Level:</strong> {ingredient.ReorderLevel}</li>
                </ul>";
    }
}
