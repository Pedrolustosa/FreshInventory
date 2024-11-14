using MediatR;

namespace FreshInventory.Application.CQRS.Commands.DeleteRecipe
{
    public record SoftDeleteRecipeCommand(int RecipeId) : IRequest;
}
