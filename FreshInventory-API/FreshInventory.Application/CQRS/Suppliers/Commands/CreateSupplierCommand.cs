using FreshInventory.Application.DTO.SupplierDTO;
using MediatR;

namespace FreshInventory.Application.Features.Suppliers.Commands
{
    public class CreateSupplierCommand : IRequest<SupplierReadDto>
    {
        public SupplierCreateDto SupplierCreateDto { get; set; }

        public CreateSupplierCommand(SupplierCreateDto supplierCreateDto)
        {
            SupplierCreateDto = supplierCreateDto;
        }
    }
}
