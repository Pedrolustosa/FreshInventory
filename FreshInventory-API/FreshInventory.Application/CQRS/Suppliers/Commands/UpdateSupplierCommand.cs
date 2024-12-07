using MediatR;
using FreshInventory.Application.DTO.SupplierDTO;

namespace FreshInventory.Application.CQRS.Supplier.Commands
{
    public class UpdateSupplierCommand : IRequest<SupplierReadDto>
    {
        public int SupplierId { get; set; }
        public SupplierUpdateDto SupplierUpdateDto { get; set; }

        public UpdateSupplierCommand(int supplierId, SupplierUpdateDto supplierUpdateDto)
        {
            SupplierId = supplierId;
            SupplierUpdateDto = supplierUpdateDto;
        }
    }
}
