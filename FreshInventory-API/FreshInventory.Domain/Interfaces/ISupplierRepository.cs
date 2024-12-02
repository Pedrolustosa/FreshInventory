using FreshInventory.Domain.Entities;

namespace FreshInventory.Domain.Interfaces
{
    public interface ISupplierRepository
    {
        Task<Supplier> GetByIdAsync(int id);
        Task<IEnumerable<Supplier>> GetAllAsync();
        Task<Supplier> AddAsync(Supplier supplier);
        Task<bool> UpdateAsync(Supplier supplier);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasLinkedIngredientsAsync(int supplierId);
    }
}
