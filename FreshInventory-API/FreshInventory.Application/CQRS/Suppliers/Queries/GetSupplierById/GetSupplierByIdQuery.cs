using MediatR;
using FreshInventory.Application.DTO.SupplierDTO;

namespace FreshInventory.Application.CQRS.Suppliers.Queries.GetSupplierById
{
    public class GetSupplierByIdQuery : IRequest<SupplierDto>
    {
        public int Id { get; set; }
    }
}
