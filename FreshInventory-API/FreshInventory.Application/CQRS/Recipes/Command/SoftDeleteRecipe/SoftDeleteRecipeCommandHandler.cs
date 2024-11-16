using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.CQRS.Commands.DeleteRecipe
{
    public class SoftDeleteRecipeCommandHandler : IRequestHandler<SoftDeleteRecipeCommand>
    {
        private readonly IRecipeRepository _repository;
        private readonly ILogger<SoftDeleteRecipeCommandHandler> _logger;

        public SoftDeleteRecipeCommandHandler(IRecipeRepository repository, ILogger<SoftDeleteRecipeCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task Handle(SoftDeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var recipe = await _repository.GetByIdAsync(request.RecipeId)
                    ?? throw new ServiceException($"Recipe with ID {request.RecipeId} not found.");

                recipe.SetActiveStatus(false);
                await _repository.UpdateAsync(recipe);

                _logger.LogInformation("Recipe with ID {RecipeId} was soft deleted successfully.", request.RecipeId);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error while attempting to soft delete recipe with ID {RecipeId}.", request.RecipeId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while attempting to soft delete recipe with ID {RecipeId}.", request.RecipeId);
                throw new Exception("An unexpected error occurred while attempting to soft delete the recipe.", ex);
            }
        }
    }
}
