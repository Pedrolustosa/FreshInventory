using AutoMapper;
using MediatR;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Recipes.Queries;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Recipes.Handlers;

public class GetRecipeByIdQueryHandler(IRecipeRepository recipeRepository, IMapper mapper, ILogger<GetRecipeByIdQueryHandler> logger) : IRequestHandler<GetRecipeByIdQuery, RecipeReadDto>
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetRecipeByIdQueryHandler> _logger = logger;

    public async Task<RecipeReadDto> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Fetching recipe with ID {RecipeId}.", request.RecipeId);

            var recipe = await _recipeRepository.GetRecipeByIdAsync(request.RecipeId);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {RecipeId} not found.", request.RecipeId);
                throw new KeyNotFoundException($"Recipe with ID {request.RecipeId} not found.");
            }

            var recipeDto = _mapper.Map<RecipeReadDto>(recipe);
            _logger.LogInformation("Successfully retrieved recipe with ID {RecipeId}.", request.RecipeId);

            return recipeDto;
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Recipe with ID {RecipeId} was not found.", request.RecipeId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving recipe with ID {RecipeId}.", request.RecipeId);
            throw;
        }
    }
}
