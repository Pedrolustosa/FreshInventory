using MediatR;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Common;

namespace FreshInventory.Application.CQRS.Suppliers.Queries.GetAllSuppliers
{
    public class GetAllSuppliersQuery : IRequest<PagedList<SupplierDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchQuery { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
