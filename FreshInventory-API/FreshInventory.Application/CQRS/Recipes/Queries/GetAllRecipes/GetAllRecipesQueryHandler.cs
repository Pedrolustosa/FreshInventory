using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO.RecipeDTO;
using FreshInventory.Application.Common;

namespace FreshInventory.Application.CQRS.Recipes.Queries.GetAllRecipes
{
    public class GetAllRecipesQueryHandler(IRecipeRepository repository, IMapper mapper, ILogger<GetAllRecipesQueryHandler> logger) : IRequestHandler<GetAllRecipesQuery, PagedList<RecipeDto>>
    {
        private readonly IRecipeRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<GetAllRecipesQueryHandler> _logger = logger;

        public async Task<PagedList<RecipeDto>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var (recipes, totalCount) = await _repository.GetAllRecipesAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.Name,
                    request.SortBy,
                    request.SortDirection);

                var recipeDtos = _mapper.Map<List<RecipeDto>>(recipes);

                _logger.LogInformation("Retrieved {Count} recipes.", totalCount);
                return new PagedList<RecipeDto>(recipeDtos, totalCount, request.PageNumber, request.PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving recipes.");
                throw new Exception("An unexpected error occurred while retrieving recipes.", ex);
            }
        }
    }
}
