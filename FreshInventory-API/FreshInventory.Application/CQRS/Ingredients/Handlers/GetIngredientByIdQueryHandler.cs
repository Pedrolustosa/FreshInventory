using AutoMapper;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using MediatR;
using FreshInventory.Application.Features.Ingredients.Queries;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Ingredients.Handlers;

public class GetIngredientByIdQueryHandler(IIngredientRepository ingredientRepository, IMapper mapper, ILogger<GetIngredientByIdQueryHandler> logger) : IRequestHandler<GetIngredientByIdQuery, IngredientReadDto>
{
    private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetIngredientByIdQueryHandler> _logger = logger;

    public async Task<IngredientReadDto> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Retrieving ingredient with ID {IngredientId}.", request.IngredientId);

            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(request.IngredientId);

            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {IngredientId} not found.", request.IngredientId);
                throw new RepositoryException($"Ingredient with ID {request.IngredientId} not found.");
            }

            _logger.LogInformation("Successfully retrieved ingredient with ID {IngredientId}.", request.IngredientId);
            return _mapper.Map<IngredientReadDto>(ingredient);
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception occurred while retrieving ingredient with ID {IngredientId}.", request.IngredientId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving ingredient with ID {IngredientId}.", request.IngredientId);
            throw;
        }
    }
}
