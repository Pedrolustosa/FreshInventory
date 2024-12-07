using MediatR;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Recipes.Commands;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Recipes.Handlers;

public class DeleteRecipeCommandHandler(IRecipeRepository recipeRepository, ILogger<DeleteRecipeCommandHandler> logger) : IRequestHandler<DeleteRecipeCommand, bool>
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly ILogger<DeleteRecipeCommandHandler> _logger = logger;

    public async Task<bool> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
    {
        if (request.RecipeId <= 0)
        {
            _logger.LogWarning("Invalid RecipeId: {RecipeId} provided for deletion.", request.RecipeId);
            throw new ArgumentException("Recipe ID must be greater than zero.", nameof(request.RecipeId));
        }

        try
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(request.RecipeId);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe with ID {RecipeId} not found.", request.RecipeId);
                throw new KeyNotFoundException($"Recipe with ID {request.RecipeId} not found.");
            }

            var isDeleted = await _recipeRepository.DeleteRecipeAsync(request.RecipeId);

            if (isDeleted)
            {
                _logger.LogInformation("Recipe with ID {RecipeId} deleted successfully.", request.RecipeId);
            }
            else
            {
                _logger.LogWarning("Failed to delete recipe with ID {RecipeId}.", request.RecipeId);
            }

            return isDeleted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting recipe with ID {RecipeId}.", request.RecipeId);
            throw;
        }
    }
}
