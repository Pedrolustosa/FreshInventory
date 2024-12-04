using FreshInventory.Domain.Common.Models;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Infrastructure.Data.Repositories
{
    public class SupplierRepository(FreshInventoryDbContext dbContext, ILogger<SupplierRepository> logger) : ISupplierRepository
    {
        private readonly FreshInventoryDbContext _dbContext = dbContext;
        private readonly ILogger<SupplierRepository> _logger = logger;

        public async Task<Supplier> GetByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving supplier with ID {Id}.", id);
                var supplier = await _dbContext.Suppliers
                    .Include(s => s.Ingredients)
                    .FirstOrDefaultAsync(s => s.Id == id);

                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {Id} not found.", id);
                }

                return supplier;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving supplier with ID {Id}.", id);
                throw;
            }
        }

        public async Task<PaginatedList<Supplier>> GetAllPagedAsync(int pageNumber, int pageSize)
        {
            try
            {
                _logger.LogInformation("Retrieving paginated suppliers. Page: {PageNumber}, Size: {PageSize}.", pageNumber, pageSize);

                var totalCount = await _dbContext.Suppliers.CountAsync();
                var suppliers = await _dbContext.Suppliers
                    .Include(s => s.Ingredients)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                _logger.LogInformation("Successfully retrieved {Count} suppliers on page {PageNumber}.", suppliers.Count, pageNumber);

                return new PaginatedList<Supplier>(suppliers, totalCount, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving paginated suppliers.");
                throw;
            }
        }

        public async Task<Supplier> AddAsync(Supplier supplier)
        {
            try
            {
                _logger.LogInformation("Adding new supplier: {SupplierName}.", supplier.Name);
                _dbContext.Suppliers.Add(supplier);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Supplier added successfully with ID {Id}.", supplier.Id);
                return supplier;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new supplier: {SupplierName}.", supplier.Name);
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Supplier supplier)
        {
            try
            {
                _logger.LogInformation("Updating supplier with ID {Id}.", supplier.Id);
                _dbContext.Suppliers.Update(supplier);
                var result = await _dbContext.SaveChangesAsync() > 0;

                if (result)
                {
                    _logger.LogInformation("Supplier with ID {Id} updated successfully.", supplier.Id);
                }
                else
                {
                    _logger.LogWarning("Failed to update supplier with ID {Id}.", supplier.Id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating supplier with ID {Id}.", supplier.Id);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting supplier with ID {Id}.", id);

                var supplier = await GetByIdAsync(id);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {Id} not found for deletion.", id);
                    return false;
                }

                _dbContext.Suppliers.Remove(supplier);
                var result = await _dbContext.SaveChangesAsync() > 0;

                if (result)
                {
                    _logger.LogInformation("Supplier with ID {Id} deleted successfully.", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete supplier with ID {Id}.", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting supplier with ID {Id}.", id);
                throw;
            }
        }

        public async Task<bool> HasLinkedIngredientsAsync(int supplierId)
        {
            try
            {
                _logger.LogInformation("Checking if supplier with ID {SupplierId} has linked ingredients.", supplierId);
                var result = await _dbContext.Ingredients.AnyAsync(i => i.SupplierId == supplierId);

                if (result)
                {
                    _logger.LogInformation("Supplier with ID {SupplierId} has linked ingredients.", supplierId);
                }
                else
                {
                    _logger.LogInformation("Supplier with ID {SupplierId} has no linked ingredients.", supplierId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking linked ingredients for supplier with ID {SupplierId}.", supplierId);
                throw;
            }
        }
    }
}
