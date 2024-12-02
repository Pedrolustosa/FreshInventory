using FreshInventory.Application.DTO.RecipeDTO;
using MediatR;

namespace FreshInventory.Application.Features.Recipes.Queries
{
    public class GetRecipeByIdQuery : IRequest<RecipeReadDto>
    {
        public int RecipeId { get; set; }

        public GetRecipeByIdQuery(int recipeId)
        {
            RecipeId = recipeId;
        }
    }
}
