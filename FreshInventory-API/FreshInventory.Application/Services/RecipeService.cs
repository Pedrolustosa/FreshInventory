using MediatR;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Common;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.Exceptions;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.CQRS.Commands.CreateRecipe;
using FreshInventory.Application.CQRS.Commands.UpdateRecipe;
using FreshInventory.Application.CQRS.Commands.DeleteRecipe;
using FreshInventory.Application.CQRS.Commands.ReactivateRecipe;
using FreshInventory.Application.CQRS.Commands.ReserveIngredients;

namespace FreshInventory.Application.Services;

public class RecipeService(IRecipeRepository recipeRepository, IMediator mediator, ILogger<RecipeService> logger) : IRecipeService
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<RecipeService> _logger = logger;
    private readonly IRecipeRepository _recipeRepository = recipeRepository;

    public async Task<PagedList<RecipeDto>> GetAllRecipesAsync(int pageNumber, int pageSize, string? name = null, string? sortBy = null, string? sortDirection = null)
    {
        try
        {
            var (recipes, totalCount) = await _recipeRepository.GetAllRecipesAsync(pageNumber, pageSize, name, sortBy, sortDirection);
            var recipeDtos = recipes.Select(r => new RecipeDto(r.Id, r.Name, default, default)).ToList();
            return new PagedList<RecipeDto>(recipeDtos, totalCount, pageNumber, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving recipes.");
            throw new ServiceException("An error occurred while retrieving recipes.", ex);
        }
    }

    public async Task<RecipeDto> CreateRecipeAsync(RecipeCreateDto recipeCreateDto)
    {
        try
        {
            var command = new CreateRecipeCommand(recipeCreateDto);
            return await _mediator.Send(command);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating recipe '{Name}'.", recipeCreateDto.Name);
            throw new ServiceException("An error occurred while creating the recipe.", ex);
        }
    }

    public async Task<RecipeDto> UpdateRecipeAsync(RecipeUpdateDto recipeUpdateDto)
    {
        try
        {
            var command = new UpdateRecipeCommand(recipeUpdateDto);
            return await _mediator.Send(command);
        }
        catch (ServiceException ex)
        {
            _logger.LogError(ex, "An error occurred while updating recipe with ID {Id}.", recipeUpdateDto.Id);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating recipe with ID {Id}.", recipeUpdateDto.Id);
            throw new ServiceException("An error occurred while updating the recipe.", ex);
        }
    }

    public async Task DeleteRecipeAsync(int recipeId)
    {
        try
        {
            var command = new SoftDeleteRecipeCommand(recipeId);
            await _mediator.Send(command);
        }
        catch (ServiceException ex)
        {
            _logger.LogError(ex, "An error occurred while deleting recipe with ID {Id}.", recipeId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting recipe with ID {Id}.", recipeId);
            throw new ServiceException("An error occurred while deleting the recipe.", ex);
        }
    }

    public async Task ReactivateRecipeAsync(int recipeId)
    {
        try
        {
            var command = new ReactivateRecipeCommand(recipeId);
            await _mediator.Send(command);
        }
        catch (ServiceException ex)
        {
            _logger.LogError(ex, "An error occurred while reactivating recipe with ID {Id}.", recipeId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while reactivating recipe with ID {Id}.", recipeId);
            throw new ServiceException("An error occurred while reactivating the recipe.", ex);
        }
    }

    public async Task<bool> ReserveIngredientsAsync(int recipeId)
    {
        try
        {
            var command = new ReserveIngredientsForRecipeCommand(recipeId);
            return await _mediator.Send(command);
        }
        catch (ServiceException ex)
        {
            _logger.LogError(ex, "An error occurred while reserving ingredients for recipe with ID {Id}.", recipeId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while reserving ingredients for recipe with ID {Id}.", recipeId);
            throw new ServiceException("An error occurred while reserving ingredients.", ex);
        }
    }

    public async Task<RecipeDto> GetRecipeByIdAsync(int recipeId)
    {
        try
        {
            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {Id} not found.", recipeId);
                throw new ServiceException($"Recipe with ID {recipeId} not found.");
            }
            return new RecipeDto(recipe.Id, recipe.Name, default, default);
        }
        catch (ServiceException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving recipe with ID {Id}.", recipeId);
            throw new ServiceException("An error occurred while retrieving the recipe.", ex);
        }
    }
}
