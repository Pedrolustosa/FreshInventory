using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.DTO.SupplierDTO;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.CQRS.Suppliers.Command.CreateSupplier;
using FreshInventory.Application.CQRS.Suppliers.Command.DeleteSupplier;
using FreshInventory.Application.CQRS.Suppliers.Command.UpdateSupplier;
using FreshInventory.Application.CQRS.Suppliers.Queries.GetAllSuppliers;
using FreshInventory.Application.CQRS.Suppliers.Queries.GetSupplierById;
using FreshInventory.Application.Common;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.Services
{
    public class SupplierService(IMediator mediator, IMapper mapper, ILogger<SupplierService> logger) : ISupplierService
    {
        private readonly IMediator _mediator = mediator;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<SupplierService> _logger = logger;

        public async Task<SupplierDto> CreateSupplierAsync(SupplierCreateDto supplierCreateDto)
        {
            try
            {
                _logger.LogInformation("Attempting to create a new supplier: {Name}", supplierCreateDto.Name);

                var command = _mapper.Map<CreateSupplierCommand>(supplierCreateDto);
                var createdSupplier = await _mediator.Send(command);

                _logger.LogInformation("Supplier {Name} created successfully with ID {Id}.", createdSupplier.Name, createdSupplier.Id);

                return _mapper.Map<SupplierDto>(createdSupplier);
            }
            catch (ServiceException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating supplier: {Name}.", supplierCreateDto.Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating supplier: {Name}.", supplierCreateDto.Name);
                throw new ServiceException("An error occurred while creating the supplier.", ex);
            }
        }

        public async Task<SupplierDto> GetSupplierByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Retrieving supplier with ID {Id}.", id);

                var query = new GetSupplierByIdQuery { Id = id };
                var supplier = await _mediator.Send(query);

                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {Id} not found.", id);
                    return null;
                }

                _logger.LogInformation("Supplier with ID {Id} retrieved successfully.", id);

                return _mapper.Map<SupplierDto>(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving supplier with ID {Id}.", id);
                throw new ServiceException("An error occurred while retrieving the supplier.", ex);
            }
        }

        public async Task<PagedList<SupplierDto>> GetAllSuppliersAsync(
        int pageNumber,
        int pageSize,
        string? name = null,
        string? sortBy = null,
        string? sortDirection = null)
        {
            try
            {
                _logger.LogInformation("Retrieving suppliers with pagination: PageNumber={PageNumber}, PageSize={PageSize}, Name={Name}, SortBy={SortBy}, SortDirection={SortDirection}.",
                    pageNumber, pageSize, name, sortBy, sortDirection);

                var query = new GetAllSuppliersQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SearchQuery = name,
                    SortBy = sortBy,
                    SortDirection = sortDirection
                };

                var suppliers = await _mediator.Send(query);

                _logger.LogInformation("Retrieved {Count} suppliers successfully.", suppliers.TotalCount);

                return suppliers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving suppliers with pagination.");
                throw new ServiceException("An error occurred while retrieving suppliers.", ex);
            }
        }

        public async Task UpdateSupplierAsync(SupplierUpdateDto supplierUpdateDto)
        {
            try
            {
                _logger.LogInformation("Attempting to update supplier with ID {Id}.", supplierUpdateDto.Id);

                var command = _mapper.Map<UpdateSupplierCommand>(supplierUpdateDto);
                await _mediator.Send(command);

                _logger.LogInformation("Supplier with ID {Id} updated successfully.", supplierUpdateDto.Id);
            }
            catch (ServiceException ex)
            {
                _logger.LogWarning(ex, "Validation error while updating supplier with ID {Id}.", supplierUpdateDto.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating supplier with ID {Id}.", supplierUpdateDto.Id);
                throw new ServiceException("An error occurred while updating the supplier.", ex);
            }
        }

        public async Task DeleteSupplierAsync(int id)
        {
            try
            {
                _logger.LogInformation("Attempting to delete supplier with ID {Id}.", id);

                var command = new DeleteSupplierCommand { Id = id };
                await _mediator.Send(command);

                _logger.LogInformation("Supplier with ID {Id} deleted successfully.", id);
            }
            catch (ServiceException ex)
            {
                _logger.LogWarning(ex, "Validation error while deleting supplier with ID {Id}.", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting supplier with ID {Id}.", id);
                throw new ServiceException("An error occurred while deleting the supplier.", ex);
            }
        }
    }
}
