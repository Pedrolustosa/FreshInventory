using AutoMapper;
using MediatR;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Recipes.Commands;

namespace FreshInventory.Application.Features.Recipes.Handlers
{
    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, RecipeReadDto>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public CreateRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<RecipeReadDto> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = _mapper.Map<Recipe>(request.RecipeCreateDto);
            await _recipeRepository.AddRecipeAsync(recipe);
            return _mapper.Map<RecipeReadDto>(recipe);
        }
    }
}
