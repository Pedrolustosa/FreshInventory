using FreshInventory.Domain.Common.Models;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Infrastructure.Data.Services;

public class IngredientRepository(FreshInventoryDbContext context, ILogger<IngredientRepository> logger) : IIngredientRepository
{
    private readonly FreshInventoryDbContext _context = context;
    private readonly ILogger<IngredientRepository> _logger = logger;

    public async Task<Ingredient> GetIngredientByIdAsync(int ingredientId)
    {
        try
        {
            _logger.LogInformation("Attempting to retrieve ingredient with ID {IngredientId}", ingredientId);

            var ingredient = await _context.Ingredients
                .Include(i => i.Supplier)
                .FirstOrDefaultAsync(i => i.Id == ingredientId);

            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {IngredientId} not found", ingredientId);
            }
            else
            {
                _logger.LogInformation("Successfully retrieved ingredient with ID {IngredientId}", ingredientId);
            }

            return ingredient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving ingredient with ID {IngredientId}", ingredientId);
            throw;
        }
    }

    public async Task<PaginatedList<Ingredient>> GetAllIngredientsPagedAsync(int pageNumber, int pageSize)
    {
        try
        {
            _logger.LogInformation("Retrieving paginated ingredients. Page: {PageNumber}, PageSize: {PageSize}", pageNumber, pageSize);

            var totalCount = await _context.Ingredients.CountAsync();

            var items = await _context.Ingredients
                .Include(i => i.Supplier)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            _logger.LogInformation("Successfully retrieved {Count} ingredients on page {PageNumber}. Total count: {TotalCount}.", items.Count, pageNumber, totalCount);

            return new PaginatedList<Ingredient>(items, totalCount, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving paginated ingredients.");
            throw;
        }
    }


    public async Task AddIngredientAsync(Ingredient ingredient)
    {
        try
        {
            _logger.LogInformation("Attempting to add a new ingredient: {Name}", ingredient.Name);

            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully added ingredient: {Name}", ingredient.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding ingredient: {Name}", ingredient.Name);
            throw;
        }
    }

    public async Task UpdateIngredientAsync(Ingredient ingredient)
    {
        try
        {
            _logger.LogInformation("Attempting to update ingredient with ID {IngredientId}", ingredient.Id);

            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully updated ingredient with ID {IngredientId}", ingredient.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating ingredient with ID {IngredientId}", ingredient.Id);
            throw;
        }
    }

    public async Task<bool> DeleteIngredientAsync(int ingredientId)
    {
        try
        {
            _logger.LogInformation("Attempting to delete ingredient with ID {IngredientId}", ingredientId);

            var ingredient = await _context.Ingredients.FindAsync(ingredientId);
            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {IngredientId} not found for deletion", ingredientId);
                return false;
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted ingredient with ID {IngredientId}", ingredientId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting ingredient with ID {IngredientId}", ingredientId);
            throw;
        }
    }
}
