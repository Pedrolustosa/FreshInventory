using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Features.Suppliers.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Common.Models;

namespace FreshInventory.Application.Features.Suppliers.Handlers;

public class GetAllSuppliersPagedQueryHandler : IRequestHandler<GetAllSuppliersPagedQuery, PaginatedList<SupplierReadDto>>
{
    private readonly ISupplierRepository _supplierRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllSuppliersPagedQueryHandler> _logger;

    public GetAllSuppliersPagedQueryHandler(ISupplierRepository supplierRepository, IMapper mapper, ILogger<GetAllSuppliersPagedQueryHandler> logger)
    {
        _supplierRepository = supplierRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedList<SupplierReadDto>> Handle(GetAllSuppliersPagedQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving paginated suppliers. Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);

            var suppliers = await _supplierRepository.GetAllPagedAsync(request.PageNumber, request.PageSize);
            if (!suppliers.Items.Any())
            {
                _logger.LogWarning("No suppliers found for the given page.");
                return new PaginatedList<SupplierReadDto>(Enumerable.Empty<SupplierReadDto>(), 0, request.PageNumber, request.PageSize);
            }

            var supplierDtos = _mapper.Map<IEnumerable<SupplierReadDto>>(suppliers.Items);
            _logger.LogInformation("Successfully retrieved paginated suppliers. Page: {PageNumber}, Size: {PageSize}", request.PageNumber, request.PageSize);

            return new PaginatedList<SupplierReadDto>(supplierDtos, suppliers.TotalCount, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving paginated suppliers.");
            throw;
        }
    }
}

