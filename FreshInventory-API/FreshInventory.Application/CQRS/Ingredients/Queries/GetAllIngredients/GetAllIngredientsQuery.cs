using MediatR;
using FreshInventory.Application.DTO;
using FreshInventory.Application.Common;

namespace FreshInventory.Application.CQRS.Ingredients.Queries.GetAllIngredients;

public class GetAllIngredientsQuery(
    int pageNumber = 1,
    int pageSize = 10,
    string? name = null,
    string? category = null,
    string? sortBy = null,
    string? sortDirection = null) : IRequest<PagedList<IngredientDto>>
{
    public int PageNumber { get; } = pageNumber > 0 ? pageNumber : 1;
    public int PageSize { get; } = pageSize > 0 ? pageSize : 10;
    public string? Name { get; } = name;
    public string? Category { get; } = category;
    public string? SortBy { get; } = sortBy;
    public string? SortDirection { get; } = sortDirection;
}
