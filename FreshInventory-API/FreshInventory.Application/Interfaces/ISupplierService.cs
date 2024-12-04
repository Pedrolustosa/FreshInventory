using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Domain.Common.Models;

namespace FreshInventory.Application.Interfaces
{
    public interface ISupplierService
    {
        Task<SupplierReadDto> CreateSupplierAsync(SupplierCreateDto supplierCreateDto);
        Task<SupplierReadDto> UpdateSupplierAsync(int supplierId, SupplierUpdateDto supplierUpdateDto);
        Task<SupplierReadDto> GetSupplierByIdAsync(int supplierId);
        Task<PaginatedList<SupplierReadDto>> GetAllSuppliersPagedAsync(int pageNumber, int pageSize);
        Task<bool> DeleteSupplierAsync(int supplierId);
    }
}
