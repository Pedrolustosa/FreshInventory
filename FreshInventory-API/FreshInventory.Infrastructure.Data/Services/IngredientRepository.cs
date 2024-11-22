using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;
using AutoMapper;
using FreshInventory.Domain.Enums;

namespace FreshInventory.Infrastructure.Data.Services;

public class IngredientRepository(ApplicationDbContext context, ILogger<IngredientRepository> logger) : IIngredientRepository, IDisposable
{
    private bool _disposed = false;
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred when adding ingredient '{Name}'.", ingredient.Name);
            throw new RepositoryException("An unexpected error occurred while adding the ingredient.", ex);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred when updating ingredient '{Name}'.", ingredient.Name);
            throw new RepositoryException("An unexpected error occurred while updating the ingredient.", ex);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred when deleting ingredient with ID {Id}.", id);
            throw new RepositoryException("An unexpected error occurred while deleting the ingredient.", ex);
        }
    }

    public async Task<(IEnumerable<Ingredient> Items, int TotalCount)> GetAllIngredientsAsync(
        int pageNumber,
        int pageSize,
        string? name = null,
        string? category = null,
        string? sortBy = null,
        string? sortDirection = null)
    {
        try
        {
            var query = _context.Ingredients
                .Include(x => x.Supplier)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(i => i.Name.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(category) && Enum.TryParse<Category>(category, true, out var parsedCategory))
            {
                query = query.Where(i => i.Category == parsedCategory);
            }

            query = sortBy?.ToLower() switch
            {
                "name" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(i => i.Name) : query.OrderBy(i => i.Name),
                "quantity" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(i => i.Quantity) : query.OrderBy(i => i.Quantity),
                "category" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(i => i.Category) : query.OrderBy(i => i.Category),
                _ => query.OrderBy(i => i.Id),
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving ingredients from the database.");
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

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing) _context.Dispose();
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
