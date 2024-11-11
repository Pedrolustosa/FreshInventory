using MediatR;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Commands.DeleteIngredient;

public class DeleteIngredientCommandHandler(
    IIngredientRepository repository,
    ILogger<DeleteIngredientCommandHandler> logger) : IRequestHandler<DeleteIngredientCommand>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly ILogger<DeleteIngredientCommandHandler> _logger = logger;

    public async Task Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ingredient = await _repository.GetByIdAsync(request.Id)
                ?? throw new ServiceException($"Ingredient with ID {request.Id} not found.");

            await _repository.DeleteAsync(request.Id);

            _logger.LogInformation("Ingredient with ID {Id} deleted successfully.", request.Id);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting ingredient with ID {Id}.", request.Id);
            throw;
        }
        catch (ServiceException ex)
        {
            _logger.LogError(ex, "Service error while deleting ingredient with ID {Id}.", request.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting ingredient with ID {Id}.", request.Id);
            throw new Exception("An unexpected error occurred while deleting the ingredient.", ex);
        }
    }
}
