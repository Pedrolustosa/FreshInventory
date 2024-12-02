using AutoMapper;
using MediatR;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Recipes.Commands;

namespace FreshInventory.Application.Features.Recipes.Handlers
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, RecipeReadDto>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<RecipeReadDto> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(request.RecipeId);
            if (recipe == null)
                throw new KeyNotFoundException($"Recipe with ID {request.RecipeId} not found.");

            _mapper.Map(request.RecipeUpdateDto, recipe);
            await _recipeRepository.UpdateRecipeAsync(recipe);
            return _mapper.Map<RecipeReadDto>(recipe);
        }
    }
}
