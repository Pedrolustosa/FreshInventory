using MediatR;
using FreshInventory.Application.DTO.RecipeDTO;

namespace FreshInventory.Application.CQRS.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQuery : IRequest<RecipeDto>
    {
        public int RecipeId { get; set; }
    }
}
