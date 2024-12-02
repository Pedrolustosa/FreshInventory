using FreshInventory.Application.DTO.RecipeDTO;
using MediatR;

namespace FreshInventory.Application.Features.Recipes.Commands
{
    public class CreateRecipeCommand : IRequest<RecipeReadDto>
    {
        public RecipeCreateDto RecipeCreateDto { get; set; }

        public CreateRecipeCommand(RecipeCreateDto recipeCreateDto)
        {
            RecipeCreateDto = recipeCreateDto;
        }
    }
}
