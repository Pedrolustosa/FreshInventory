using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Common;
using FreshInventory.Application.Exceptions;
using FreshInventory.Application.DTO.IngredientDTO;

namespace FreshInventory.Application.CQRS.Ingredients.Queries.GetAllIngredients;

public class GetAllIngredientsQueryHandler : IRequestHandler<GetAllIngredientsQuery, PagedList<IngredientDto>>
{
    private readonly IIngredientRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllIngredientsQueryHandler> _logger;

    public GetAllIngredientsQueryHandler(
        IIngredientRepository repository,
        IMapper mapper,
        ILogger<GetAllIngredientsQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<IngredientDto>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
            {
                _logger.LogWarning("Invalid pagination parameters: PageNumber={PageNumber}, PageSize={PageSize}.", request.PageNumber, request.PageSize);
                throw new QueryException("Invalid pagination parameters.");
            }

            var (items, totalCount) = await _repository.GetAllIngredientsAsync(
                request.PageNumber,
                request.PageSize,
                request.Name,
                request.Category,
                request.SortBy,
                request.SortDirection);

            if (items == null || !items.Any())
            {
                _logger.LogWarning("No ingredients found with the specified filters.");
                return new PagedList<IngredientDto>(new List<IngredientDto>(), 0, request.PageNumber, request.PageSize);
            }

            var itemsDto = _mapper.Map<List<IngredientDto>>(items);
            var pagedList = new PagedList<IngredientDto>(itemsDto, totalCount, request.PageNumber, request.PageSize);

            _logger.LogInformation("Successfully retrieved {Count} ingredients from database.", totalCount);

            return pagedList;
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "Repository error occurred while retrieving ingredients.");
            throw new QueryException("An error occurred while retrieving ingredients.", ex);
        }
        catch (QueryException ex)
        {
            _logger.LogError(ex, "Query exception occurred while retrieving ingredients.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving ingredients.");
            throw new QueryException("An unexpected error occurred while retrieving ingredients.", ex);
        }
    }
}
