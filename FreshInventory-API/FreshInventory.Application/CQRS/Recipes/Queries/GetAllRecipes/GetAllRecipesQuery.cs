using MediatR;
using FreshInventory.Application.Common;
using FreshInventory.Application.DTO.RecipeDTO;

namespace FreshInventory.Application.CQRS.Recipes.Queries.GetAllRecipes
{
    public class GetAllRecipesQuery : IRequest<PagedList<RecipeDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Name { get; set; }
        public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
