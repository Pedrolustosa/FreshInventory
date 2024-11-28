using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;

namespace FreshInventory.Infrastructure.Data.Services;

public class RecipeRepository(ApplicationDbContext context, ILogger<RecipeRepository> logger) : IRecipeRepository, IDisposable
{
    private bool _disposed = false;
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<RecipeRepository> _logger = logger;

    public async Task<(IEnumerable<Recipe> Recipes, int TotalCount)> GetAllRecipesAsync(int pageNumber, int pageSize, string? name = null, string? sortBy = null, string? sortDirection = null)
    {
        try
        {
            var query = _context.Recipes.Include(x => x.Ingredients).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(r => r.Name.Contains(name));

            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                var isDescending = sortDirection?.ToLower() == "desc";
                query = sortBy.ToLower() switch
                {
                    "name" => isDescending ? query.OrderByDescending(r => r.Name) : query.OrderBy(r => r.Name),
                    _ => query.OrderBy(r => r.Id)
                };
            }
            else
            {
                query = query.OrderBy(r => r.Id);
            }

            var totalCount = await query.CountAsync();
            var recipes = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            _logger.LogInformation("Retrieved {Count} recipes from database.", recipes.Count());
            return (recipes, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving recipes.");
            throw new RepositoryException("An error occurred while retrieving recipes.", ex);
        }
    }

    public async Task<Recipe> AddAsync(Recipe recipe)
    {
        try
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Recipe '{Name}' added to the database.", recipe.Name);
            return recipe;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update failed when adding recipe '{Name}'.", recipe.Name);
            throw new RepositoryException("An error occurred while adding the recipe.", ex);
        }
    }

    public async Task UpdateAsync(Recipe recipe)
    {
        try
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Recipe '{Name}' updated in the database.", recipe.Name);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update failed when updating recipe '{Name}'.", recipe.Name);
            throw new RepositoryException("An error occurred while updating the recipe.", ex);
        }
    }

    public async Task DeleteAsync(int recipeId)
    {
        try
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {Id} not found for deletion.", recipeId);
                throw new RepositoryException($"Recipe with ID {recipeId} not found.");
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation("Recipe with ID {Id} logically deleted.", recipeId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update failed when logically deleting recipe with ID {Id}.", recipeId);
            throw new RepositoryException("An error occurred while deleting the recipe.", ex);
        }
    }

    public async Task ReactivateAsync(int recipeId)
    {
        try
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {Id} not found for reactivation.", recipeId);
                throw new RepositoryException($"Recipe with ID {recipeId} not found.");
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation("Recipe with ID {Id} reactivated.", recipeId);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update failed when reactivating recipe with ID {Id}.", recipeId);
            throw new RepositoryException("An error occurred while reactivating the recipe.", ex);
        }
    }

    public async Task<Recipe> GetByIdAsync(int recipeId)
    {
        try
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == recipeId && !r.IsDeleted);

            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {Id} not found.", recipeId);
                throw new RepositoryException($"Recipe with ID {recipeId} not found.");
            }
            _logger.LogInformation("Recipe with ID {Id} retrieved from database.", recipeId);
            return recipe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving recipe with ID {Id}.", recipeId);
            throw new RepositoryException("An error occurred while retrieving the recipe.", ex);
        }
    }


    public async Task<bool> ReserveIngredientsAsync(int recipeId)
    {
        try
        {
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == recipeId && !r.IsDeleted);

            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {Id} not found.", recipeId);
                throw new RepositoryException($"Recipe with ID {recipeId} not found.");
            }

            foreach (var ingredient in recipe.Ingredients)
            {
                var inventoryIngredient = await _context.Ingredients.FindAsync(ingredient.IngredientId);
                if (inventoryIngredient == null || inventoryIngredient.Quantity < ingredient.Quantity)
                {
                    _logger.LogWarning("Insufficient quantity for ingredient in recipe ID {Id}.", recipeId);
                    return false;
                }
                inventoryIngredient.UpdateQuantity(inventoryIngredient.Quantity - ingredient.Quantity);
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation("Ingredients for recipe ID {Id} reserved successfully.", recipeId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while reserving ingredients for recipe ID {Id}.", recipeId);
            throw new RepositoryException("An error occurred while reserving ingredients.", ex);
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