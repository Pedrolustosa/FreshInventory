using AutoMapper;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.Features.Recipes.Commands;
using FreshInventory.Application.Features.Recipes.Queries;
using FreshInventory.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<RecipeService> _logger;

        public RecipeService(IMediator mediator, IMapper mapper, ILogger<RecipeService> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RecipeReadDto> CreateRecipeAsync(RecipeCreateDto recipeCreateDto)
        {
            if (recipeCreateDto == null)
            {
                _logger.LogWarning("Received null data for recipe creation.");
                throw new ArgumentNullException(nameof(recipeCreateDto), "RecipeCreateDto cannot be null.");
            }

            try
            {
                var command = _mapper.Map<CreateRecipeCommand>(recipeCreateDto);
                var recipeReadDto = await _mediator.Send(command);

                _logger.LogInformation("Recipe created successfully: {Name}", recipeCreateDto.Name);
                return recipeReadDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during recipe creation.");
                throw;
            }
        }

        public async Task<RecipeReadDto> UpdateRecipeAsync(int recipeId, RecipeUpdateDto recipeUpdateDto)
        {
            if (recipeUpdateDto == null)
            {
                _logger.LogWarning("Received null data for recipe update.");
                throw new ArgumentNullException(nameof(recipeUpdateDto), "RecipeUpdateDto cannot be null.");
            }

            try
            {
                var command = new UpdateRecipeCommand(recipeId, recipeUpdateDto);
                var updatedRecipe = await _mediator.Send(command);

                _logger.LogInformation("Recipe with ID {RecipeId} updated successfully.", recipeId);
                return updatedRecipe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during recipe update.");
                throw;
            }
        }

        public async Task<bool> DeleteRecipeAsync(int recipeId)
        {
            try
            {
                var command = new DeleteRecipeCommand(recipeId);
                var isDeleted = await _mediator.Send(command);

                if (isDeleted)
                {
                    _logger.LogInformation("Recipe with ID {RecipeId} deleted successfully.", recipeId);
                }
                else
                {
                    _logger.LogWarning("Failed to delete recipe with ID {RecipeId}.", recipeId);
                }

                return isDeleted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during recipe deletion.");
                throw;
            }
        }

        public async Task<RecipeReadDto> GetRecipeByIdAsync(int recipeId)
        {
            try
            {
                var query = new GetRecipeByIdQuery(recipeId);
                var recipe = await _mediator.Send(query);

                if (recipe != null)
                {
                    _logger.LogInformation("Recipe with ID {RecipeId} retrieved successfully.", recipeId);
                }
                else
                {
                    _logger.LogWarning("Recipe with ID {RecipeId} not found.", recipeId);
                }

                return recipe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving recipe with ID {RecipeId}.", recipeId);
                throw;
            }
        }

        public async Task<IEnumerable<RecipeReadDto>> GetAllRecipesAsync()
        {
            try
            {
                var query = new GetAllRecipesQuery();
                var recipes = await _mediator.Send(query);

                _logger.LogInformation("Retrieved all recipes successfully.");
                return recipes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all recipes.");
                throw;
            }
        }
    }
}
