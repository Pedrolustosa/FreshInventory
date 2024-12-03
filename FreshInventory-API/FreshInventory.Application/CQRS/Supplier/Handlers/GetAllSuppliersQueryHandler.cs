using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Features.Suppliers.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Suppliers.Handlers;

public class GetAllSuppliersQueryHandler(ISupplierRepository supplierRepository, IMapper mapper, ILogger<GetAllSuppliersQueryHandler> logger) : IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierReadDto>>
{
    private readonly ISupplierRepository _supplierRepository = supplierRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetAllSuppliersQueryHandler> _logger = logger;

    public async Task<IEnumerable<SupplierReadDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving all suppliers...");

            var suppliers = await _supplierRepository.GetAllAsync();
            if (suppliers == null || !suppliers.Any())
            {
                _logger.LogWarning("No suppliers found.");
                return Enumerable.Empty<SupplierReadDto>();
            }

            var supplierDtos = _mapper.Map<IEnumerable<SupplierReadDto>>(suppliers);
            _logger.LogInformation("Successfully retrieved {Count} suppliers.", supplierDtos.Count());
            return supplierDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all suppliers.");
            throw;
        }
    }
}
