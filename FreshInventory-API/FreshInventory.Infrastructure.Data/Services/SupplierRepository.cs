using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FreshInventory.Infrastructure.Data.Repositories
{
    public class SupplierRepository(FreshInventoryDbContext dbContext) : ISupplierRepository
    {
        private readonly FreshInventoryDbContext _dbContext = dbContext;

        public async Task<Supplier> GetByIdAsync(int id)
        {
            return await _dbContext.Suppliers
                .Include(s => s.Ingredients)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Supplier>> GetAllAsync()
        {
            return await _dbContext.Suppliers
                .Include(s => s.Ingredients)
                .ToListAsync();
        }

        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            _dbContext.Suppliers.Add(supplier);
            await _dbContext.SaveChangesAsync();
            return supplier;
        }

        public async Task<bool> UpdateAsync(Supplier supplier)
        {
            _dbContext.Suppliers.Update(supplier);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var supplier = await GetByIdAsync(id);
            if (supplier == null)
            {
                return false;
            }

            _dbContext.Suppliers.Remove(supplier);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> HasLinkedIngredientsAsync(int supplierId)
        {
            return await _dbContext.Ingredients.AnyAsync(i => i.SupplierId == supplierId);
        }
    }
}
