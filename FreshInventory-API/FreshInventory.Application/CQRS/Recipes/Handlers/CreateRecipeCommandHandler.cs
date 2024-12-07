using AutoMapper;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.Features.Recipes.Commands;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.CQRS.Recipes.Handlers
{
    public class CreateRecipeCommandHandler(
        IRecipeRepository recipeRepository,
        IMapper mapper,
        ILogger<CreateRecipeCommandHandler> logger) : IRequestHandler<CreateRecipeCommand, RecipeReadDto>
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

            if (request.RecipeCreateDto.Ingredients == null || !request.RecipeCreateDto.Ingredients.Any())
            {
                _logger.LogWarning("A recipe must have at least one ingredient.");
                throw new ArgumentException("A recipe must have at least one ingredient.");
            }

            try
            {
                var recipe = _mapper.Map<Recipe>(request.RecipeCreateDto);

                recipe.SetCreatedDate();

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
}