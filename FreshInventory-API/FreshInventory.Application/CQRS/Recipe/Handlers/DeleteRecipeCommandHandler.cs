using MediatR;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Features.Recipes.Commands;

namespace FreshInventory.Application.Features.Recipes.Handlers
{
    public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeCommand, bool>
    {
        private readonly IRecipeRepository _recipeRepository;

        public DeleteRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<bool> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(request.RecipeId);
            if (recipe == null)
            {
                throw new KeyNotFoundException($"Recipe with ID {request.RecipeId} not found.");
            }

            return await _recipeRepository.DeleteRecipeAsync(request.RecipeId);
        }
    }
}
