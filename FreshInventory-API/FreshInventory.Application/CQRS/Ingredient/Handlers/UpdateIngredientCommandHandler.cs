using AutoMapper;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Application.Features.Ingredients.Commands;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Ingredients.Handlers;

public class UpdateIngredientCommandHandler(IIngredientRepository ingredientRepository, IMapper mapper, ILogger<UpdateIngredientCommandHandler> logger) : IRequestHandler<UpdateIngredientCommand, IngredientReadDto>
{
    private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UpdateIngredientCommandHandler> _logger = logger;

    public async Task<IngredientReadDto> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Attempting to update ingredient with ID {IngredientId}.", request.IngredientId);

            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(request.IngredientId);
            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {IngredientId} not found.", request.IngredientId);
                throw new RepositoryException($"Ingredient with ID {request.IngredientId} not found.");
            }

            _logger.LogInformation("Mapping update data to ingredient entity for ID {IngredientId}.", request.IngredientId);
            _mapper.Map(request.IngredientUpdateDto, ingredient);

            await _ingredientRepository.UpdateIngredientAsync(ingredient);

            _logger.LogInformation("Ingredient with ID {IngredientId} updated successfully.", request.IngredientId);
            return _mapper.Map<IngredientReadDto>(ingredient);
        }
        catch (RepositoryException ex)
        {
            _logger.LogWarning(ex, "Repository exception occurred while updating ingredient with ID {IngredientId}.", request.IngredientId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating ingredient with ID {IngredientId}.", request.IngredientId);
            throw;
        }
    }
}
