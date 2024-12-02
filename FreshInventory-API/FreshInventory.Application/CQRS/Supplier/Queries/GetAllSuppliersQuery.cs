using System.Collections.Generic;
using FreshInventory.Application.DTO.SupplierDTO;
using MediatR;

namespace FreshInventory.Application.Features.Suppliers.Queries
{
    public class GetAllSuppliersQuery : IRequest<IEnumerable<SupplierReadDto>>
    {
    }
}
