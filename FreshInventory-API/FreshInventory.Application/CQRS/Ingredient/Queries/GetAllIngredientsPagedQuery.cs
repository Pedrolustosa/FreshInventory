using MediatR;
using FreshInventory.Domain.Common.Models;
using FreshInventory.Application.DTO.IngredientDTO;

namespace FreshInventory.Application.Features.Ingredients.Queries;

public class GetAllIngredientsPagedQuery : IRequest<PaginatedList<IngredientReadDto>>
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetAllIngredientsPagedQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
}
