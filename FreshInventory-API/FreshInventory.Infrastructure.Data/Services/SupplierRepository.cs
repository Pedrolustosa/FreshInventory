using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Infrastructure.Data.Context;

namespace FreshInventory.Infrastructure.Data.Services;

public class SupplierRepository(ApplicationDbContext context, ILogger<SupplierRepository> logger) : ISupplierRepository, IDisposable
{
    private bool _disposed = false;
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<SupplierRepository> _logger = logger;

    public async Task AddAsync(Supplier supplier)
    {
        try
        {
            await _context.Suppliers.AddAsync(supplier);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Supplier '{Name}' added to database.", supplier.Name);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to add supplier '{Name}' due to a database error.", supplier.Name);
            throw new RepositoryException($"An error occurred while adding supplier '{supplier.Name}'.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while adding supplier '{Name}'.", supplier.Name);
            throw new RepositoryException($"An unexpected error occurred while adding supplier '{supplier.Name}'.", ex);
        }
    }

    public async Task UpdateAsync(Supplier supplier)
    {
        try
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Supplier '{Name}' updated in database.", supplier.Name);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency error occurred while updating supplier '{Name}'.", supplier.Name);
            throw new RepositoryException($"The supplier '{supplier.Name}' might have been modified or deleted. Please try again.", ex);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to update supplier '{Name}' due to a database error.", supplier.Name);
            throw new RepositoryException($"An error occurred while updating supplier '{supplier.Name}'.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while updating supplier '{Name}'.", supplier.Name);
            throw new RepositoryException($"An unexpected error occurred while updating supplier '{supplier.Name}'.", ex);
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            var supplier = await GetByIdAsync(id);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {Id} not found for deletion.", id);
                throw new RepositoryException($"Supplier with ID {id} not found.");
            }

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Supplier with ID {Id} deleted from database.", id);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Failed to delete supplier with ID {Id} due to a database error.", id);
            throw new RepositoryException($"An error occurred while deleting supplier with ID {id}.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while deleting supplier with ID {Id}.", id);
            throw new RepositoryException($"An unexpected error occurred while deleting supplier with ID {id}.", ex);
        }
    }

    public async Task<(IEnumerable<Supplier> Items, int TotalCount)> GetAllSupplierAsync(int pageNumber, int pageSize, string? name = null, string? category = null, string? sortBy = null, string? sortDirection = null)
    {
        try
        {
            var query = _context.Suppliers.AsNoTracking();

            // Filtering
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(s => s.Name.Contains(name) || s.Email.Contains(name));
            }

            // Sorting
            query = sortBy?.ToLower() switch
            {
                "name" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(s => s.Name) : query.OrderBy(s => s.Name),
                "createddate" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(s => s.CreatedDate) : query.OrderBy(s => s.CreatedDate),
                "updateddate" => sortDirection?.ToLower() == "desc" ? query.OrderByDescending(s => s.UpdatedDate) : query.OrderBy(s => s.UpdatedDate),
                _ => query.OrderBy(s => s.Id),
            };

            var totalCount = await query.CountAsync();
            _logger.LogInformation("Retrieved {Count} suppliers from database.", totalCount);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving suppliers from the database.");
            throw new RepositoryException("An error occurred while retrieving suppliers.", ex);
        }
    }

    public async Task<Supplier> GetByIdAsync(int id)
    {
        try
        {
            var supplier = await _context.Suppliers.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {Id} not found.", id);
                throw new RepositoryException($"Supplier with ID {id} not found.");
            }
            _logger.LogInformation("Supplier with ID {Id} retrieved from database.", id);
            return supplier;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving supplier with ID {Id} from database.", id);
            throw new RepositoryException($"An error occurred while retrieving supplier with ID {id}.", ex);
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
