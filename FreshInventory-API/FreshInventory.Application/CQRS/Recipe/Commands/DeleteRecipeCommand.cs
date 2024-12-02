using MediatR;

namespace FreshInventory.Application.Features.Recipes.Commands
{
    public class DeleteRecipeCommand : IRequest<bool>
    {
        public int RecipeId { get; }

        public DeleteRecipeCommand(int recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
