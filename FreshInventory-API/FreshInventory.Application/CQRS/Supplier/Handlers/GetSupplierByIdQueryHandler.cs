using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Features.Suppliers.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Suppliers.Handlers;

public class GetSupplierByIdQueryHandler(ISupplierRepository supplierRepository, IMapper mapper, ILogger<GetSupplierByIdQueryHandler> logger) : IRequestHandler<GetSupplierByIdQuery, SupplierReadDto>
{
    private readonly ISupplierRepository _supplierRepository = supplierRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetSupplierByIdQueryHandler> _logger = logger;

    public async Task<SupplierReadDto> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving supplier with ID {SupplierId}...", request.Id);

            var supplier = await _supplierRepository.GetByIdAsync(request.Id);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {SupplierId} not found.", request.Id);
                throw new KeyNotFoundException($"Supplier with ID {request.Id} not found.");
            }

            var supplierDto = _mapper.Map<SupplierReadDto>(supplier);
            _logger.LogInformation("Supplier with ID {SupplierId} retrieved successfully.", request.Id);
            return supplierDto;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Key not found exception while retrieving supplier with ID {SupplierId}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving supplier with ID {SupplierId}.", request.Id);
            throw;
        }
    }
}
