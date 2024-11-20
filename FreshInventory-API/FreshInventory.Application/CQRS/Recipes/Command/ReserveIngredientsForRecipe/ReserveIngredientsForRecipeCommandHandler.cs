using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.CQRS.Commands.ReserveIngredients
{
    public class ReserveIngredientsForRecipeCommandHandler : IRequestHandler<ReserveIngredientsForRecipeCommand, bool>
    {
        private readonly IRecipeRepository _repository;
        private readonly IIngredientRepository _ingredientRepository;
        private readonly ILogger<ReserveIngredientsForRecipeCommandHandler> _logger;

        public ReserveIngredientsForRecipeCommandHandler(
            IRecipeRepository repository,
            IIngredientRepository ingredientRepository,
            ILogger<ReserveIngredientsForRecipeCommandHandler> logger)
        {
            _repository = repository;
            _ingredientRepository = ingredientRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(ReserveIngredientsForRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var recipe = await _repository.GetByIdAsync(request.RecipeId)
                    ?? throw new ServiceException($"Recipe with ID {request.RecipeId} not found.");

                foreach (var recipeIngredient in recipe.Ingredients)
                {
                    var ingredient = await _ingredientRepository.GetByIdAsync(recipeIngredient.IngredientId)
                        ?? throw new ServiceException($"Ingredient with ID {recipeIngredient.IngredientId} not found.");

                    if (ingredient.Quantity < recipeIngredient.Quantity)
                    {
                        _logger.LogWarning("Insufficient quantity for ingredient '{IngredientName}' in recipe ID {RecipeId}.", ingredient.Name, request.RecipeId);
                        throw new ServiceException($"Insufficient quantity for ingredient '{ingredient.Name}'.");
                    }

                    ingredient.ReduceQuantity(recipeIngredient.Quantity);
                    await _ingredientRepository.UpdateAsync(ingredient);

                    _logger.LogInformation("Reserved {Quantity} of ingredient '{IngredientName}' for recipe ID {RecipeId}.", recipeIngredient.Quantity, ingredient.Name, request.RecipeId);
                }

                _logger.LogInformation("All ingredients successfully reserved for recipe ID {RecipeId}.", request.RecipeId);
                return true;
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error while reserving ingredients for recipe ID {RecipeId}.", request.RecipeId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while reserving ingredients for recipe ID {RecipeId}.", request.RecipeId);
                throw new ServiceException("An unexpected error occurred while reserving ingredients.", ex);
            }
        }
    }
}
