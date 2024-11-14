using FreshInventory.Application.DTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Commands.CreateRecipe
{
    public record CreateRecipeCommand(RecipeCreateDto RecipeCreateDto) : IRequest<RecipeDto>;
}