using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;
using MediatR;

namespace FreshInventory.Application.CQRS.Commands.DeleteRecipe
{
    public class SoftDeleteRecipeCommandHandler : IRequestHandler<SoftDeleteRecipeCommand>
    {
        private readonly IRecipeRepository _repository;

        public SoftDeleteRecipeCommandHandler(IRecipeRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(SoftDeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _repository.GetByIdAsync(request.RecipeId)
                ?? throw new ServiceException($"Recipe with ID {request.RecipeId} not found.");

            recipe.SetActiveStatus(false);
            await _repository.UpdateAsync(recipe);
        }
    }
}
