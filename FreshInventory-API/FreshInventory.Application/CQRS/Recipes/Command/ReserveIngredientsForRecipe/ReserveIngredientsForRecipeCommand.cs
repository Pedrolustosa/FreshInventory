using MediatR;

namespace FreshInventory.Application.CQRS.Commands.ReserveIngredients
{
    public record ReserveIngredientsForRecipeCommand(int RecipeId) : IRequest<bool>;
}