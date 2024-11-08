using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Common;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Queries.GetAllIngredients;

public class GetAllIngredientsQueryHandler(
    IIngredientRepository repository,
    IMapper mapper,
    ILogger<GetAllIngredientsQueryHandler> logger) : IRequestHandler<GetAllIngredientsQuery, PagedList<IngredientDto>>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetAllIngredientsQueryHandler> _logger = logger;

    public async Task<PagedList<IngredientDto>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var (items, totalCount) = await _repository.GetAllIngredientsAsync(
                request.PageNumber,
                request.PageSize,
                request.Name,
                request.Category,
                request.SortBy,
                request.SortDirection);

            var itemsDto = _mapper.Map<List<IngredientDto>>(items);
            var pagedList = new PagedList<IngredientDto>(itemsDto, totalCount, request.PageNumber, request.PageSize);

            _logger.LogInformation("Retrieved {Count} ingredients from database.", totalCount);

            return pagedList;
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving ingredients.");
            throw new QueryException("An error occurred while retrieving ingredients.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving ingredients.");
            throw new QueryException("An unexpected error occurred while retrieving ingredients.", ex);
        }
    }
}
