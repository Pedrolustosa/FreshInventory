using FreshInventory.Application.DTO.SupplierDTO;
using MediatR;

namespace FreshInventory.Application.Features.Suppliers.Queries
{
    public class GetSupplierByIdQuery : IRequest<SupplierReadDto>
    {
        public int Id { get; set; }

        public GetSupplierByIdQuery(int id)
        {
            Id = id;
        }
    }
}
