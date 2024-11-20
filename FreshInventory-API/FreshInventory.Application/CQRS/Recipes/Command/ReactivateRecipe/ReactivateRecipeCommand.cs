using MediatR;

namespace FreshInventory.Application.CQRS.Commands.ReactivateRecipe
{
    public class ReactivateRecipeCommand : IRequest
    {
        public int RecipeId { get; set; }

        public ReactivateRecipeCommand(int recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
