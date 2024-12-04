using MediatR;
using AutoMapper;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Ingredients.Queries;
using FreshInventory.Domain.Common.Models;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Ingredients.Handlers;

public class GetAllIngredientsPagedQueryHandler : IRequestHandler<GetAllIngredientsPagedQuery, PaginatedList<IngredientReadDto>>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllIngredientsPagedQueryHandler> _logger;

    public GetAllIngredientsPagedQueryHandler(IIngredientRepository ingredientRepository, IMapper mapper, ILogger<GetAllIngredientsPagedQueryHandler> logger)
    {
        _ingredientRepository = ingredientRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedList<IngredientReadDto>> Handle(GetAllIngredientsPagedQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving paginated ingredients. Page: {PageNumber}, PageSize: {PageSize}", request.PageNumber, request.PageSize);

            var ingredients = await _ingredientRepository.GetAllIngredientsPagedAsync(request.PageNumber, request.PageSize);

            var ingredientDtos = _mapper.Map<List<IngredientReadDto>>(ingredients.Items);

            _logger.LogInformation("Successfully retrieved {Count} ingredients on page {PageNumber}.", ingredientDtos.Count, request.PageNumber);

            return new PaginatedList<IngredientReadDto>(ingredientDtos, ingredients.TotalCount, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving paginated ingredients.");
            throw;
        }
    }
}
