using AutoMapper;
using MediatR;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Recipes.Queries;

namespace FreshInventory.Application.Features.Recipes.Handlers
{
    public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, RecipeReadDto>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public GetRecipeByIdQueryHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<RecipeReadDto> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetRecipeByIdAsync(request.RecipeId);
            if (recipe == null)
                throw new KeyNotFoundException($"Recipe with ID {request.RecipeId} not found.");

            return _mapper.Map<RecipeReadDto>(recipe);
        }
    }
}
