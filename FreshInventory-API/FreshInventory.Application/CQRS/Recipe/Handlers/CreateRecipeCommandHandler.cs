using AutoMapper;
using MediatR;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Recipes.Commands;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Recipes.Handlers;

public class CreateRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper, ILogger<CreateRecipeCommandHandler> logger) : IRequestHandler<CreateRecipeCommand, RecipeReadDto>
{
    private readonly IRecipeRepository _recipeRepository = recipeRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CreateRecipeCommandHandler> _logger = logger;

    public async Task<RecipeReadDto> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        if (request.RecipeCreateDto == null)
        {
            _logger.LogWarning("Received null RecipeCreateDto in CreateRecipeCommand.");
            throw new ArgumentNullException(nameof(request.RecipeCreateDto), "RecipeCreateDto cannot be null.");
        }

        try
        {
            var recipe = _mapper.Map<Recipe>(request.RecipeCreateDto);
            await _recipeRepository.AddRecipeAsync(recipe);

            _logger.LogInformation("Recipe created successfully with ID: {RecipeId}, Name: {RecipeName}", recipe.Id, recipe.Name);

            return _mapper.Map<RecipeReadDto>(recipe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating a recipe: {RecipeName}", request.RecipeCreateDto.Name);
            throw;
        }
    }
}
