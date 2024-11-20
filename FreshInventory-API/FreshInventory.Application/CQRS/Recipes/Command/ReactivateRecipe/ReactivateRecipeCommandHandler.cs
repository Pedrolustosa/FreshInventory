using MediatR;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.CQRS.Commands.ReactivateRecipe
{
    public class ReactivateRecipeCommandHandler : IRequestHandler<ReactivateRecipeCommand>
    {
        private readonly IRecipeRepository _repository;
        private readonly ILogger<ReactivateRecipeCommandHandler> _logger;

        public ReactivateRecipeCommandHandler(IRecipeRepository repository, ILogger<ReactivateRecipeCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(ReactivateRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var recipe = await _repository.GetByIdAsync(request.RecipeId)
                    ?? throw new ServiceException($"Recipe with ID {request.RecipeId} not found.");

                //recipe.SetActiveStatus(true);
                await _repository.UpdateAsync(recipe);

                _logger.LogInformation("Recipe with ID {Id} reactivated successfully.", request.RecipeId);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Error while reactivating recipe with ID {Id}.", request.RecipeId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while reactivating recipe with ID {Id}.", request.RecipeId);
                throw new ServiceException("An unexpected error occurred while reactivating the recipe.", ex);
            }
        }
    }
}
