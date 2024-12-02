using MediatR;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Ingredients.Commands;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Ingredients.Handlers;

public class DeleteIngredientCommandHandler(IIngredientRepository ingredientRepository, ILogger<DeleteIngredientCommandHandler> logger) : IRequestHandler<DeleteIngredientCommand, bool>
{
    private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
    private readonly ILogger<DeleteIngredientCommandHandler> _logger = logger;

    public async Task<bool> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
    {
        if (request.IngredientId <= 0)
        {
            _logger.LogWarning("Invalid Ingredient ID {IngredientId} received for deletion.", request.IngredientId);
            throw new ArgumentException("Ingredient ID must be greater than zero.", nameof(request.IngredientId));
        }

        try
        {
            _logger.LogInformation("Retrieving ingredient with ID {IngredientId} for deletion.", request.IngredientId);
            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(request.IngredientId);

            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {IngredientId} not found.", request.IngredientId);
                throw new KeyNotFoundException($"Ingredient with ID {request.IngredientId} not found.");
            }

            _logger.LogInformation("Deleting ingredient with ID {IngredientId}.", request.IngredientId);
            var result = await _ingredientRepository.DeleteIngredientAsync(request.IngredientId);

            if (result)
            {
                _logger.LogInformation("Ingredient with ID {IngredientId} deleted successfully.", request.IngredientId);
            }
            else
            {
                _logger.LogWarning("Failed to delete ingredient with ID {IngredientId}.", request.IngredientId);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting ingredient with ID {IngredientId}.", request.IngredientId);
            throw;
        }
    }
}
