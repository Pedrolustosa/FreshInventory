using AutoMapper;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.Features.Recipes.Commands;
using FreshInventory.Application.Features.Recipes.Queries;
using FreshInventory.Application.Interfaces;
using FreshInventory.Domain.Common.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Services;

public class RecipeService(IMediator mediator, IMapper mapper, ILogger<RecipeService> logger) : IRecipeService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<RecipeService> _logger = logger;

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
            _logger.LogError(ex, "Error during recipe creation for Name: {Name}.", recipeCreateDto?.Name);
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
            _logger.LogError(ex, "Error during recipe update for ID: {RecipeId}.", recipeId);
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
            _logger.LogError(ex, "Error during recipe deletion for ID: {RecipeId}.", recipeId);
            throw;
        }
    }

    public async Task<RecipeReadDto> GetRecipeByIdAsync(int recipeId)
    {
        if (recipeId <= 0)
        {
            _logger.LogWarning("Invalid recipe ID received: {RecipeId}", recipeId);
            throw new ArgumentException("Recipe ID must be greater than zero.", nameof(recipeId));
        }

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
            _logger.LogError(ex, "Error retrieving recipe with ID {RecipeId}.", recipeId);
            throw;
        }
    }

    public async Task<PaginatedList<RecipeReadDto>> GetAllRecipesPagedAsync(int pageNumber, int pageSize)
    {
        try
        {
            var query = new GetAllRecipesPagedQuery(pageNumber, pageSize);
            var paginatedRecipes = await _mediator.Send(query);

            _logger.LogInformation("Successfully retrieved paginated recipes. Page: {PageNumber}, Size: {PageSize}", pageNumber, pageSize);

            return paginatedRecipes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving paginated recipes.");
            throw;
        }
    }

}
