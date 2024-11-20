using FreshInventory.Domain.Entities;

namespace FreshInventory.Domain.Interfaces
{
    public interface ISupplierRepository
    {
        Task<(IEnumerable<Supplier> Items, int TotalCount)> GetAllSupplierAsync(
            int pageNumber,
            int pageSize,
            string? name = null,
            string? category = null,
            string? sortBy = null,
            string? sortDirection = null);

        Task<Supplier?> GetByIdAsync(int id);
        Task AddAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(int id);
    }
}
