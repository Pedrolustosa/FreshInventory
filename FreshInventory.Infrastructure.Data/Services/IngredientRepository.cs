using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;

namespace FreshInventory.Infrastructure.Data.Services;

public class IngredientRepository(ApplicationDbContext context, ILogger<IngredientRepository> logger) : IIngredientRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<IngredientRepository> _logger = logger;

    public async Task AddAsync(Ingredient ingredient)
    {
        try
        {
            await _context.Ingredients.AddAsync(ingredient);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Ingredient '{Name}' saved to database.", ingredient.Name);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update failed when adding ingredient '{Name}'.", ingredient.Name);
            throw new RepositoryException("An error occurred while adding the ingredient.", ex);
        }
    }

    public async Task UpdateAsync(Ingredient ingredient)
    {
        try
        {
            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Ingredient '{Name}' updated in database.", ingredient.Name);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error when updating ingredient '{Name}'.", ingredient.Name);
            throw new RepositoryException("An error occurred while updating the ingredient. It might have been modified or deleted.", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update failed when updating ingredient '{Name}'.", ingredient.Name);
            throw new RepositoryException("An error occurred while updating the ingredient.", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Ingredient with ID {Id} deleted from database.", id);
            }
            else
            {
                _logger.LogWarning("Ingredient with ID {Id} not found for deletion.", id);
                throw new RepositoryException($"Ingredient with ID {id} not found.");
            }
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update failed when deleting ingredient with ID {Id}.", id);
            throw new RepositoryException("An error occurred while deleting the ingredient.", ex);
        }
    }

    public async Task<IEnumerable<Ingredient>> GetAllAsync()
    {
        try
        {
            var ingredients = await _context.Ingredients.AsNoTracking().ToListAsync();
            _logger.LogInformation("Retrieved {Count} ingredients from database.", ingredients.Count);
            return ingredients;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving ingredients from database.");
            throw new RepositoryException("An error occurred while retrieving ingredients.", ex);
        }
    }

    public async Task<Ingredient> GetByIdAsync(int id)
    {
        try
        {
            var ingredient = await _context.Ingredients.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} not found.", id);
                throw new RepositoryException($"Ingredient with ID {id} not found.");
            }
            _logger.LogInformation("Ingredient with ID {Id} retrieved from database.", id);
            return ingredient;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving ingredient with ID {Id} from database.", id);
            throw new RepositoryException("An error occurred while retrieving the ingredient.", ex);
        }
    }
}
