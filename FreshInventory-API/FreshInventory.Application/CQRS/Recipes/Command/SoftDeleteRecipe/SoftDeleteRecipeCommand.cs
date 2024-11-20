using MediatR;

namespace FreshInventory.Application.CQRS.Commands.DeleteRecipe
{
    public class SoftDeleteRecipeCommand : IRequest
    {
        public int RecipeId { get; set; }

        public SoftDeleteRecipeCommand(int recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
