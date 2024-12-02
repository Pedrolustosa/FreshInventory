using AutoMapper;
using FreshInventory.Application.CQRS.Ingredient.Commands;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Application.Features.Ingredients.Commands;
using FreshInventory.Application.Features.Ingredients.Queries;
using FreshInventory.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<IngredientService> _logger;

        public IngredientService(IMediator mediator, IMapper mapper, ILogger<IngredientService> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IngredientReadDto> CreateIngredientAsync(IngredientCreateDto ingredientCreateDto)
        {
            if (ingredientCreateDto == null)
            {
                _logger.LogWarning("Received null data for ingredient creation.");
                throw new ArgumentNullException(nameof(ingredientCreateDto), "IngredientCreateDto cannot be null.");
            }

            try
            {
                var command = _mapper.Map<CreateIngredientCommand>(ingredientCreateDto);
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    _logger.LogWarning("Ingredient creation failed.");
                    throw new Exception("Ingredient creation failed.");
                }

                _logger.LogInformation("Ingredient created successfully: {Name}", ingredientCreateDto.Name);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during ingredient creation.");
                throw;
            }
        }

        public async Task<IEnumerable<IngredientReadDto>> GetAllIngredientsAsync()
        {
            try
            {
                var query = new GetAllIngredientsQuery();
                var result = await _mediator.Send(query);

                _logger.LogInformation("Retrieved all ingredients successfully.");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all ingredients.");
                throw;
            }
        }

        public async Task<IngredientReadDto> GetIngredientByIdAsync(int ingredientId)
        {
            if (ingredientId <= 0)
            {
                _logger.LogWarning("Invalid ingredient ID received: {IngredientId}", ingredientId);
                throw new ArgumentException("Ingredient ID must be greater than zero.", nameof(ingredientId));
            }

            try
            {
                var query = new GetIngredientByIdQuery(ingredientId);
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    _logger.LogWarning("Ingredient with ID {IngredientId} not found.", ingredientId);
                    throw new KeyNotFoundException($"Ingredient with ID {ingredientId} not found.");
                }

                _logger.LogInformation("Ingredient with ID {IngredientId} retrieved successfully.", ingredientId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving ingredient with ID {IngredientId}.", ingredientId);
                throw;
            }
        }

        public async Task<IngredientReadDto> UpdateIngredientAsync(int ingredientId, IngredientUpdateDto ingredientUpdateDto)
        {
            if (ingredientUpdateDto == null)
            {
                _logger.LogWarning("Received null data for ingredient update.");
                throw new ArgumentNullException(nameof(ingredientUpdateDto), "IngredientUpdateDto cannot be null.");
            }

            try
            {
                var command = new UpdateIngredientCommand(ingredientId, ingredientUpdateDto);
                var result = await _mediator.Send(command);

                if (result == null)
                {
                    _logger.LogWarning("Failed to update ingredient with ID {IngredientId}.", ingredientId);
                    throw new Exception("Failed to update ingredient.");
                }

                _logger.LogInformation("Ingredient with ID {IngredientId} updated successfully.", ingredientId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating ingredient with ID {IngredientId}.", ingredientId);
                throw;
            }
        }

        public async Task<bool> DeleteIngredientAsync(int ingredientId)
        {
            if (ingredientId <= 0)
            {
                _logger.LogWarning("Invalid ingredient ID received: {IngredientId}", ingredientId);
                throw new ArgumentException("Ingredient ID must be greater than zero.", nameof(ingredientId));
            }

            try
            {
                var command = new DeleteIngredientCommand(ingredientId);
                var result = await _mediator.Send(command);

                if (!result)
                {
                    _logger.LogWarning("Failed to delete ingredient with ID {IngredientId}.", ingredientId);
                }

                _logger.LogInformation("Ingredient with ID {IngredientId} deleted successfully.", ingredientId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting ingredient with ID {IngredientId}.", ingredientId);
                throw;
            }
        }
    }
}
