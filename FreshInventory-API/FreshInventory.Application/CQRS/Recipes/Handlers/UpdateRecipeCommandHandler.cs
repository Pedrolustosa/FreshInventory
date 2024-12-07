using AutoMapper;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.Features.Recipes.Commands;
using FreshInventory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.CQRS.Recipes.Handlers
{
    public class UpdateRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper, ILogger<UpdateRecipeCommandHandler> logger)
        : IRequestHandler<UpdateRecipeCommand, RecipeReadDto>
    {
        private readonly IRecipeRepository _recipeRepository = recipeRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateRecipeCommandHandler> _logger = logger;

        public async Task<RecipeReadDto> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            if (request.RecipeUpdateDto == null)
            {
                _logger.LogWarning("Received null RecipeUpdateDto in UpdateRecipeCommand.");
                throw new ArgumentNullException(nameof(request.RecipeUpdateDto), "RecipeUpdateDto cannot be null.");
            }

            try
            {
                _logger.LogInformation("Updating recipe with ID {RecipeId}.", request.RecipeId);

                var recipe = await _recipeRepository.GetRecipeByIdAsync(request.RecipeId);
                if (recipe == null)
                {
                    _logger.LogWarning("Recipe with ID {RecipeId} not found.", request.RecipeId);
                    throw new KeyNotFoundException($"Recipe with ID {request.RecipeId} not found.");
                }

                _mapper.Map(request.RecipeUpdateDto, recipe);

                recipe.UpdateTimestamp();

                await _recipeRepository.UpdateRecipeAsync(recipe);

                _logger.LogInformation("Recipe with ID {RecipeId} updated successfully.", request.RecipeId);

                return _mapper.Map<RecipeReadDto>(recipe);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "Failed to update recipe. Recipe with ID {RecipeId} was not found.", request.RecipeId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating recipe with ID {RecipeId}.", request.RecipeId);
                throw;
            }
        }
    }
}