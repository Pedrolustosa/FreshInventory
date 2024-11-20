using FreshInventory.Application.Common;
using FreshInventory.Application.DTO.SupplierDTO;

namespace FreshInventory.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<SupplierDto> GetSupplierByIdAsync(int id);
        Task<PagedList<SupplierDto>> GetAllSuppliersAsync(
        int pageNumber,
        int pageSize,
        string? name = null,
        string? sortBy = null,
        string? sortDirection = null);

        Task<SupplierDto> CreateSupplierAsync(SupplierCreateDto supplierCreateDto);
        Task UpdateSupplierAsync(SupplierUpdateDto supplierUpdateDto);
        Task DeleteSupplierAsync(int id);
    }
}
