using AutoMapper;
using MediatR;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.Features.Recipes.Queries;
using FreshInventory.Domain.Common.Models;
using FreshInventory.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Recipes.Handlers;

public class GetAllRecipesPagedQueryHandler(IRecipeRepository recipeRepository, IMapper mapper, ILogger<GetAllRecipesPagedQueryHandler> logger) : IRequestHandler<GetAllRecipesPagedQuery, PaginatedList<RecipeReadDto>>
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetAllRecipesPagedQueryHandler> _logger = logger;

    public async Task<PaginatedList<RecipeReadDto>> Handle(GetAllRecipesPagedQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching paginated recipes. Page {PageNumber}, Size {PageSize}.", request.PageNumber, request.PageSize);

            var paginatedRecipes = await _recipeRepository.GetAllRecipesPagedAsync(request.PageNumber, request.PageSize);

            var recipes = await _recipeRepository.GetAllRecipesPagedAsync(request.PageNumber, request.PageSize);

            var recipeDtos = _mapper.Map<List<RecipeReadDto>>(recipes.Items);

            _logger.LogInformation("Successfully retrieved {Count} ingredients on page {PageNumber}.", recipeDtos.Count, request.PageNumber);

            return new PaginatedList<RecipeReadDto>(recipeDtos, recipes.TotalCount, request.PageNumber, request.PageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving paginated recipes.");
            throw;
        }
    }
}
