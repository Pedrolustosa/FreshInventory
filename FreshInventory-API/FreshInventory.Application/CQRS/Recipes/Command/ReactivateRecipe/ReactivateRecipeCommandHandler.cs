using MediatR;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Commands.ReactivateRecipe
{
    public class ReactivateRecipeCommandHandler(IRecipeRepository repository) : IRequestHandler<ReactivateRecipeCommand>
    {
        private readonly IRecipeRepository _repository = repository;

        public async Task Handle(ReactivateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _repository.GetByIdAsync(request.RecipeId)
                ?? throw new ServiceException($"Recipe with ID {request.RecipeId} not found.");

            recipe.SetActiveStatus(true);
            await _repository.UpdateAsync(recipe);
        }
    }
}
