using FreshInventory.Application.DTO.RecipeDTO;
using MediatR;

namespace FreshInventory.Application.Features.Recipes.Queries
{
    public class GetAllRecipesQuery : IRequest<List<RecipeReadDto>>
    {
    }
}
