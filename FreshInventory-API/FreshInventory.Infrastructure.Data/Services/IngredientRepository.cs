using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Infrastructure.Data.Services
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly FreshInventoryDbContext _context;
        private readonly ILogger<IngredientRepository> _logger;

        public IngredientRepository(FreshInventoryDbContext context, ILogger<IngredientRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Ingredient> GetIngredientByIdAsync(int ingredientId)
        {
            try
            {
                return await _context.Ingredients
                    .Include(i => i.Supplier)
                    .FirstOrDefaultAsync(i => i.Id == ingredientId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving ingredient with ID {IngredientId}", ingredientId);
                throw;
            }
        }

        public async Task<IEnumerable<Ingredient>> GetAllIngredientsAsync()
        {
            try
            {
                return await _context.Ingredients
                    .Include(i => i.Supplier)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all ingredients");
                throw;
            }
        }

        public async Task AddIngredientAsync(Ingredient ingredient)
        {
            try
            {
                await _context.Ingredients.AddAsync(ingredient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding ingredient");
                throw;
            }
        }

        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            try
            {
                _context.Ingredients.Update(ingredient);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating ingredient with ID {IngredientId}", ingredient.Id);
                throw;
            }
        }

        public async Task<bool> DeleteIngredientAsync(int ingredientId)
        {
            var ingredient = await _context.Ingredients.FindAsync(ingredientId);
            if (ingredient == null)
            {
                return false;
            }

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
