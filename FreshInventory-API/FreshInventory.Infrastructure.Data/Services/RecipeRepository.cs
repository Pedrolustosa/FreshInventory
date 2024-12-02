using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FreshInventory.Infrastructure.Data.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly FreshInventoryDbContext _context;

        public RecipeRepository(FreshInventoryDbContext context)
        {
            _context = context;
        }

        public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == recipeId);
        }

        public async Task<List<Recipe>> GetAllRecipesAsync()
        {
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);

            foreach (var recipeIngredient in recipe.Ingredients)
            {
                var ingredient = await _context.Ingredients.FindAsync(recipeIngredient.IngredientId);
                if (ingredient != null)
                {
                    ingredient.DecreaseStock(recipeIngredient.QuantityRequired);
                    _context.Ingredients.Update(ingredient);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteRecipeAsync(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)
                return false;

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
