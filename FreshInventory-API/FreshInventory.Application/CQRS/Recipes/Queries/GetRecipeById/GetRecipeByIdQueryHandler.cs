using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Recipes.Queries.GetRecipeById
{
    public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, RecipeDto>
    {
        private readonly IRecipeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRecipeByIdQueryHandler> _logger;

        public GetRecipeByIdQueryHandler(IRecipeRepository repository, IMapper mapper, ILogger<GetRecipeByIdQueryHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RecipeDto> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var recipe = await _repository.GetByIdAsync(request.RecipeId);
                if (recipe == null)
                {
                    _logger.LogWarning("Recipe with ID {Id} not found.", request.RecipeId);
                    throw new QueryException($"Recipe with ID {request.RecipeId} not found.");
                }

                var recipeDto = _mapper.Map<RecipeDto>(recipe);
                _logger.LogInformation("Recipe with ID {Id} retrieved successfully.", request.RecipeId);

                return recipeDto;
            }
            catch (QueryException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving recipe with ID {Id}.", request.RecipeId);
                throw new QueryException("An unexpected error occurred while retrieving the recipe.", ex);
            }
        }
    }
}
