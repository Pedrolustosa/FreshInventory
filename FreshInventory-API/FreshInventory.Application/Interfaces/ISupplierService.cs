using FreshInventory.Application.DTO.SupplierDTO;

namespace FreshInventory.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<SupplierReadDto> CreateSupplierAsync(SupplierCreateDto supplierCreateDto);
        Task<SupplierReadDto> UpdateSupplierAsync(int supplierId, SupplierUpdateDto supplierUpdateDto);
        Task<SupplierReadDto> GetSupplierByIdAsync(int supplierId);
        Task<IEnumerable<SupplierReadDto>> GetAllSuppliersAsync();
        Task<bool> DeleteSupplierAsync(int supplierId);
    }
}
