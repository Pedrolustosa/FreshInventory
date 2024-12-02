using AutoMapper;
using MediatR;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Recipes.Queries;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Recipes.Handlers;

public class GetAllRecipesQueryHandler(IRecipeRepository recipeRepository, IMapper mapper, ILogger<GetAllRecipesQueryHandler> logger) : IRequestHandler<GetAllRecipesQuery, List<RecipeReadDto>>
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetAllRecipesQueryHandler> _logger = logger;

    public async Task<List<RecipeReadDto>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching all recipes from the repository.");
            var recipes = await _recipeRepository.GetAllRecipesAsync();

            if (recipes == null || !recipes.Any())
            {
                _logger.LogWarning("No recipes found in the repository.");
                return new List<RecipeReadDto>();
            }

            var recipeDtos = _mapper.Map<List<RecipeReadDto>>(recipes);
            _logger.LogInformation("Successfully fetched and mapped {Count} recipes.", recipeDtos.Count);

            return recipeDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all recipes.");
            throw;
        }
    }
}
