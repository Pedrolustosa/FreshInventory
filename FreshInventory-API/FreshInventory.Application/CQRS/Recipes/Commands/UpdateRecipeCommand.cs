using FreshInventory.Application.DTO.RecipeDTO;
using MediatR;

namespace FreshInventory.Application.Features.Recipes.Commands
{
    public class UpdateRecipeCommand : IRequest<RecipeReadDto>
    {
        public int RecipeId { get; set; }
        public RecipeUpdateDto RecipeUpdateDto { get; set; }

        public UpdateRecipeCommand(int recipeId, RecipeUpdateDto recipeUpdateDto)
        {
            RecipeId = recipeId;
            RecipeUpdateDto = recipeUpdateDto;
        }
    }
}
