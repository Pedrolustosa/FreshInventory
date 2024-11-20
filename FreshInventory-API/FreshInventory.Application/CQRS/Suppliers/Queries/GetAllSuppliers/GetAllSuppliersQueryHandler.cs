using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Common;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Suppliers.Queries.GetAllSuppliers
{
    public class GetAllSuppliersQueryHandler(ISupplierRepository repository, IMapper mapper, ILogger<GetAllSuppliersQueryHandler> logger) : IRequestHandler<GetAllSuppliersQuery, PagedList<SupplierDto>>
    {
        private readonly ISupplierRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllSuppliersQueryHandler> _logger = logger;

        public async Task<PagedList<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving suppliers with pagination: Page {PageNumber}, Size {PageSize}.", request.PageNumber, request.PageSize);

                var (suppliers, totalCount) = await _repository.GetAllSupplierAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SearchQuery,
                    request.SortBy,
                    request.SortDirection);

                if (suppliers == null || !suppliers.Any())
                {
                    _logger.LogWarning("No suppliers found for the given criteria.");
                    return new PagedList<SupplierDto>(new List<SupplierDto>(), totalCount, request.PageNumber, request.PageSize);
                }

                var supplierDtos = _mapper.Map<List<SupplierDto>>(suppliers);

                _logger.LogInformation("{Count} suppliers retrieved successfully.", supplierDtos.Count);
                return new PagedList<SupplierDto>(supplierDtos, totalCount, request.PageNumber, request.PageSize);
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving suppliers.");
                throw new QueryException("An error occurred while retrieving suppliers.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving suppliers.");
                throw new QueryException("An unexpected error occurred while retrieving suppliers.", ex);
            }
        }
    }
}
