using FreshInventory.Application.DTO.RecipeDTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Commands.UpdateRecipe
{
    public record UpdateRecipeCommand(RecipeUpdateDto RecipeUpdateDto) : IRequest<RecipeDto>;
}