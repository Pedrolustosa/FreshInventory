using MediatR;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Ingredients.Commands.DeleteIngredient;

public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand>
{
    private readonly IIngredientRepository _repository;
    private readonly ILogger<DeleteIngredientCommandHandler> _logger;

    public DeleteIngredientCommandHandler(
        IIngredientRepository repository,
        ILogger<DeleteIngredientCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
        {
            _logger.LogWarning("Received null request for deleting an ingredient.");
            throw new ArgumentException("Request cannot be null.");
        }

        try
        {
            var ingredient = await _repository.GetByIdAsync(request.Id);
            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} not found.", request.Id);
                throw new ServiceException($"Ingredient with ID {request.Id} not found.");
            }

            await _repository.DeleteAsync(request.Id);
            _logger.LogInformation("Ingredient with ID {Id} deleted successfully.", request.Id);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Repository error while deleting ingredient with ID {Id}.", request.Id);
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
