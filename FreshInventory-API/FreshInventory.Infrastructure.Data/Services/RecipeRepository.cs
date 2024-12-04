using FreshInventory.Domain.Common.Models;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Infrastructure.Data.Repositories;

public class RecipeRepository(FreshInventoryDbContext context, ILogger<RecipeRepository> logger) : IRecipeRepository
{
    private readonly FreshInventoryDbContext _context = context;
    private readonly ILogger<RecipeRepository> _logger = logger;

    public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
    {
        try
        {
            _logger.LogInformation("Retrieving recipe with ID {RecipeId}.", recipeId);
            var recipe = await _context.Recipes
                .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {RecipeId} not found.", recipeId);
            }
            else
            {
                _logger.LogInformation("Recipe with ID {RecipeId} retrieved successfully.", recipeId);
            }

            return recipe;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving recipe with ID {RecipeId}.", recipeId);
            throw;
        }
    }

    public async Task<PaginatedList<Recipe>> GetAllRecipesPagedAsync(int pageNumber, int pageSize)
    {
        try
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogWarning("Invalid pagination parameters. PageNumber: {PageNumber}, PageSize: {PageSize}.", pageNumber, pageSize);
                throw new ArgumentException("PageNumber and PageSize must be greater than zero.");
            }

            _logger.LogInformation("Fetching paginated recipes. PageNumber: {PageNumber}, PageSize: {PageSize}.", pageNumber, pageSize);

            _logger.LogDebug("Fetching the total count of recipes.");
            var totalCount = await _context.Recipes.CountAsync();

            _logger.LogDebug("Fetching recipes for PageNumber: {PageNumber}, PageSize: {PageSize}.", pageNumber, pageSize);
            var recipes = await _context.Recipes
                .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
                .Where(r => r.Ingredients != null && r.Ingredients.Any())
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (recipes.Count==0)
            {
                _logger.LogWarning("No recipes found for the given page. PageNumber: {PageNumber}, PageSize: {PageSize}.", pageNumber, pageSize);
                return new PaginatedList<Recipe>(new List<Recipe>(), totalCount, pageNumber, pageSize);
            }

            _logger.LogInformation("Successfully fetched paginated recipes. PageNumber: {PageNumber}, PageSize: {PageSize}, TotalCount: {TotalCount}.", pageNumber, pageSize, totalCount);

            return new PaginatedList<Recipe>(recipes, totalCount, pageNumber, pageSize);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid pagination parameters received. PageNumber: {PageNumber}, PageSize: {PageSize}.", pageNumber, pageSize);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching paginated recipes.");
            throw;
        }
    }

    public async Task AddRecipeAsync(Recipe recipe)
    {
        try
        {
            _logger.LogInformation("Adding a new recipe: {Name}", recipe.Name);

            await _context.Recipes.AddAsync(recipe);

            foreach (var recipeIngredient in recipe.Ingredients)
            {
                var ingredient = await _context.Ingredients.FindAsync(recipeIngredient.IngredientId);
                if (ingredient != null)
                {
                    ingredient.DecreaseStock(recipeIngredient.QuantityRequired);
                    _context.Ingredients.Update(ingredient);
                    _logger.LogInformation("Stock for ingredient ID {IngredientId} decreased by {Quantity}.", recipeIngredient.IngredientId, recipeIngredient.QuantityRequired);
                }
                else
                {
                    _logger.LogWarning("Ingredient ID {IngredientId} not found during recipe creation.", recipeIngredient.IngredientId);
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Recipe {Name} added successfully.", recipe.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a new recipe: {Name}.", recipe.Name);
            throw;
        }
    }

    public async Task UpdateRecipeAsync(Recipe recipe)
    {
        try
        {
            _logger.LogInformation("Updating recipe with ID {RecipeId}.", recipe.Id);
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Recipe with ID {RecipeId} updated successfully.", recipe.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating recipe with ID {RecipeId}.", recipe.Id);
            throw;
        }
    }

    public async Task<bool> DeleteRecipeAsync(int recipeId)
    {
        try
        {
            _logger.LogInformation("Deleting recipe with ID {RecipeId}.", recipeId);

            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {RecipeId} not found.", recipeId);
                return false;
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Recipe with ID {RecipeId} deleted successfully.", recipeId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting recipe with ID {RecipeId}.", recipeId);
            throw;
        }
    }
}
