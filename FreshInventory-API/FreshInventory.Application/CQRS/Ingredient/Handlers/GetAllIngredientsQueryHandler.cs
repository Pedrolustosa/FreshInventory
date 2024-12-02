using MediatR;
using AutoMapper;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Ingredients.Queries;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Ingredients.Handlers;

public class GetAllIngredientsQueryHandler(IIngredientRepository ingredientRepository, IMapper mapper, ILogger<GetAllIngredientsQueryHandler> logger) : IRequestHandler<GetAllIngredientsQuery, IEnumerable<IngredientReadDto>>
{
    private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetAllIngredientsQueryHandler> _logger = logger;

    public async Task<IEnumerable<IngredientReadDto>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving all ingredients.");
            var ingredients = await _ingredientRepository.GetAllIngredientsAsync();

            if (ingredients == null || !ingredients.Any())
            {
                _logger.LogWarning("No ingredients found.");
                return Enumerable.Empty<IngredientReadDto>();
            }

            _logger.LogInformation("Successfully retrieved {Count} ingredients.", ingredients.Count());
            return _mapper.Map<IEnumerable<IngredientReadDto>>(ingredients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving all ingredients.");
            throw;
        }
    }
}
