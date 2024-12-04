using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Domain.Common.Models;
using MediatR;

namespace FreshInventory.Application.Features.Recipes.Queries
{
    public class GetAllRecipesPagedQuery : IRequest<PaginatedList<RecipeReadDto>>
    {
        public int PageNumber { get; }
        public int PageSize { get; }

        public GetAllRecipesPagedQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
