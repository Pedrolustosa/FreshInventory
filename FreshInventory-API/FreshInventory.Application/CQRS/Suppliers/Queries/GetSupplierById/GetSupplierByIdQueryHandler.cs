using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Exceptions;
using FreshInventory.Domain.Exceptions;

namespace FreshInventory.Application.CQRS.Suppliers.Queries.GetSupplierById
{
    public class GetSupplierByIdQueryHandler(ISupplierRepository repository, IMapper mapper, ILogger<GetSupplierByIdQueryHandler> logger) : IRequestHandler<GetSupplierByIdQuery, SupplierDto>
    {
        private readonly ISupplierRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetSupplierByIdQueryHandler> _logger = logger;

        public async Task<SupplierDto> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Retrieving supplier with ID {Id}.", request.Id);

                var supplier = await _repository.GetByIdAsync(request.Id);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {Id} not found.", request.Id);
                    throw new QueryException($"Supplier with ID {request.Id} not found.");
                }

                var supplierDto = _mapper.Map<SupplierDto>(supplier);
                _logger.LogInformation("Supplier with ID {Id} retrieved successfully.", request.Id);

                return supplierDto;
            }
            catch (RepositoryException ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving supplier with ID {Id}.", request.Id);
                throw new QueryException("An error occurred while retrieving the supplier.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving supplier with ID {Id}.", request.Id);
                throw new QueryException("An unexpected error occurred while retrieving the supplier.", ex);
            }
        }
    }
}
