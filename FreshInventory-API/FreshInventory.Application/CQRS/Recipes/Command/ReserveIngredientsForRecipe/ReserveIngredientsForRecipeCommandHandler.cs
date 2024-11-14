using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;
using MediatR;

namespace FreshInventory.Application.CQRS.Commands.ReserveIngredients
{
    public class ReserveIngredientsForRecipeCommandHandler : IRequestHandler<ReserveIngredientsForRecipeCommand, bool>
    {
        private readonly IRecipeRepository _repository;
        private readonly IIngredientRepository _ingredientRepository;

        public ReserveIngredientsForRecipeCommandHandler(IRecipeRepository repository, IIngredientRepository ingredientRepository)
        {
            _repository = repository;
            _ingredientRepository = ingredientRepository;
        }

        public async Task<bool> Handle(ReserveIngredientsForRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _repository.GetByIdAsync(request.RecipeId)
                ?? throw new ServiceException($"Recipe with ID {request.RecipeId} not found.");

            foreach (var recipeIngredient in recipe.Ingredients)
            {
                var ingredient = await _ingredientRepository.GetByIdAsync(recipeIngredient.IngredientId)
                    ?? throw new ServiceException($"Ingredient with ID {recipeIngredient.IngredientId} not found.");

                if (ingredient.Quantity < recipeIngredient.QuantityRequired)
                    throw new ServiceException($"Insufficient quantity for ingredient '{ingredient.Name}'.");

                ingredient.ReduceQuantity(recipeIngredient.QuantityRequired);
                await _ingredientRepository.UpdateAsync(ingredient);
            }

            return true;
        }
    }
}