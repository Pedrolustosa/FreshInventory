using FreshInventory.Domain.Common.Models;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {RecipeId} not found.", recipeId);
                return null;
            }

            var ingredientsWithNames = await _context.Ingredients
                .Where(i => recipe.Ingredients.Keys.Contains(i.Id))
                .ToDictionaryAsync(i => i.Id, i => new { i.Name, Quantity = recipe.Ingredients[i.Id] });

            var updatedIngredients = ingredientsWithNames
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Quantity);

            recipe.SetIngredients(updatedIngredients);

            _logger.LogInformation("Recipe with ID {RecipeId} retrieved successfully.", recipeId);
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

            var totalCount = await _context.Recipes.CountAsync();
            var skip = (pageNumber - 1) * pageSize;

            var recipesData = await _context.Recipes
                .Skip(skip)
                .Take(pageSize)
                .Select(r => new
                {
                    r.Id,
                    r.Name,
                    r.Description,
                    r.Servings,
                    r.PreparationTime,
                    r.Steps,
                    Ingredients = _context.Ingredients
                        .Where(i => r.Ingredients.Keys.Contains(i.Id))
                        .ToDictionary(i => i.Id, i => r.Ingredients[i.Id])
                })
                .ToListAsync();

            var recipes = recipesData.Select(data => new Recipe(
                data.Name,
                data.Description,
                data.Servings,
                data.PreparationTime,
                data.Ingredients,
                data.Steps
            )).ToList();

            return new PaginatedList<Recipe>(recipes, totalCount, pageNumber, pageSize);
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
