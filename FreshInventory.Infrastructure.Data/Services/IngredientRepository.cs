using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;

namespace FreshInventory.Infrastructure.Data.Services;

public class IngredientRepository(ApplicationDbContext context) : IIngredientRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task AddAsync(Ingredient ingredient)
    {
        await _context.Ingredients.AddAsync(ingredient);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient != null)
        {
            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Ingredient>> GetAllAsync() => await _context.Ingredients.AsNoTracking().ToListAsync();

    public async Task<Ingredient> GetByIdAsync(int id)
    {
        var ingredient = await _context.Ingredients.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        return ingredient ?? throw new KeyNotFoundException($"Ingredient with Id '{id}' does not exist.");
    }

    public async Task UpdateAsync(Ingredient ingredient)
    {
        _context.Ingredients.Update(ingredient);
        await _context.SaveChangesAsync();
    }

    public void Dispose() => _context.Dispose();
}
