using FreshInventory.Domain.Common.Models;
using FreshInventory.Domain.Entities;

namespace FreshInventory.Domain.Interfaces
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetByIdAsync(int id);
        Task<PaginatedList<Supplier>> GetAllPagedAsync(int pageNumber, int pageSize);
        Task<Supplier> AddAsync(Supplier supplier);
        Task<bool> UpdateAsync(Supplier supplier);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasLinkedIngredientsAsync(int supplierId);
    }
}
