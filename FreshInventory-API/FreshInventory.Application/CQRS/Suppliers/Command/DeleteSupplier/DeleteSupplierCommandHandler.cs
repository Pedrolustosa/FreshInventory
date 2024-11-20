using MediatR;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Suppliers.Command.DeleteSupplier
{
    public class DeleteSupplierCommandHandler(ISupplierRepository repository, ILogger<DeleteSupplierCommandHandler> logger) : IRequestHandler<DeleteSupplierCommand>
    {
        private readonly ISupplierRepository _repository = repository;
        private readonly ILogger<DeleteSupplierCommandHandler> _logger = logger;

        public async Task Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Attempting to delete supplier with ID {Id}.", request.Id);

                var supplier = await _repository.GetByIdAsync(request.Id);
                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {Id} not found.", request.Id);
                    throw new ServiceException($"Supplier with ID {request.Id} not found.");
                }

                await _repository.DeleteAsync(request.Id);

                _logger.LogInformation("Supplier with ID {Id} deleted successfully.", request.Id);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error occurred while deleting supplier with ID {Id}.", request.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting supplier with ID {Id}.", request.Id);
                throw new ServiceException("An unexpected error occurred while deleting the supplier.", ex);
            }
        }
    }
}
