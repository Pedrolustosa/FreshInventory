using FreshInventory.Application.DTO.RecipeDTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Commands.CreateRecipe
{
    public record CreateRecipeCommand(RecipeCreateDto RecipeCreateDto) : IRequest<RecipeDto>;
}