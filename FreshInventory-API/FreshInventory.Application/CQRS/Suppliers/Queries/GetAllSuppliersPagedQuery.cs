using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Domain.Common.Models;
using MediatR;

namespace FreshInventory.Application.Features.Suppliers.Queries
{
    public class GetAllSuppliersPagedQuery : IRequest<PaginatedList<SupplierReadDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllSuppliersPagedQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber > 0 ? pageNumber : 1;
            PageSize = pageSize > 0 ? pageSize : 10;
        }
    }
}
