using MediatR;

namespace FreshInventory.Application.CQRS.Commands.ReactivateRecipe
{
    public record ReactivateRecipeCommand(int RecipeId) : IRequest;
}