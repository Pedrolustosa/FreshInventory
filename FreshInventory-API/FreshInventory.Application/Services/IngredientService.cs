using AutoMapper;
using FreshInventory.Application.CQRS.Ingredient.Commands;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Application.Features.Ingredients.Commands;
using FreshInventory.Application.Features.Ingredients.Queries;
using FreshInventory.Application.Interfaces;
using FreshInventory.Domain.Common.Models;
using FreshInventory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Services;

public class IngredientService(IMediator mediator, IMapper mapper, ILogger<IngredientService> logger, ISupplierRepository supplierRepository) : IIngredientService
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly ISupplierRepository _supplierRepository = supplierRepository;
    private readonly ILogger<IngredientService> _logger = logger;

    public async Task<IngredientReadDto> CreateIngredientAsync(IngredientCreateDto ingredientCreateDto)
    {
        if (ingredientCreateDto == null)
        {
            _logger.LogWarning("Ingredient creation failed: received null data.");
            throw new ArgumentNullException(nameof(ingredientCreateDto), "IngredientCreateDto cannot be null.");
        }

        try
        {
            var command = _mapper.Map<CreateIngredientCommand>(ingredientCreateDto);
            var result = await _mediator.Send(command);

            if (result == null)
            {
                _logger.LogWarning("Ingredient creation failed for {IngredientName}.", ingredientCreateDto.Name);
                throw new InvalidOperationException("Failed to create the ingredient.");
            }

            _logger.LogInformation("Ingredient {IngredientName} created successfully with ID {IngredientId}.", ingredientCreateDto.Name, result.Id);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating ingredient {IngredientName}.", ingredientCreateDto.Name);
            throw;
        }
    }

    public async Task<PaginatedList<IngredientReadDto>> GetAllIngredientsPagedAsync(int pageNumber, int pageSize)
    {
        try
        {
            var query = new GetAllIngredientsPagedQuery(pageNumber, pageSize);
            var result = await _mediator.Send(query);

            _logger.LogInformation("Retrieved paginated ingredients successfully. Page: {PageNumber}, PageSize: {PageSize}, TotalCount: {TotalCount}.", pageNumber, pageSize, result.TotalCount);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving paginated ingredients.");
            throw;
        }
    }


    public async Task<IngredientReadDto> GetIngredientByIdAsync(int ingredientId)
    {
        if (ingredientId <= 0)
        {
            _logger.LogWarning("Ingredient retrieval failed: invalid ID {IngredientId}.", ingredientId);
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
            _logger.LogWarning("Ingredient update failed: received null data.");
            throw new ArgumentNullException(nameof(ingredientUpdateDto), "IngredientUpdateDto cannot be null.");
        }

        try
        {
            // Validar se o SupplierId é válido
            _logger.LogInformation("Validating supplier with ID {SupplierId}.", ingredientUpdateDto.SupplierId);
            var supplierExists = await _supplierRepository.GetByIdAsync(ingredientUpdateDto.SupplierId);

            if (supplierExists == null)
            {
                _logger.LogWarning("Supplier with ID {SupplierId} does not exist.", ingredientUpdateDto.SupplierId);
                throw new ArgumentException($"Supplier with ID {ingredientUpdateDto.SupplierId} does not exist.");
            }

            var command = new UpdateIngredientCommand(ingredientId, ingredientUpdateDto);
            var result = await _mediator.Send(command);

            if (result == null)
            {
                _logger.LogWarning("Ingredient update failed for ID {IngredientId}.", ingredientId);
                throw new InvalidOperationException($"Failed to update ingredient with ID {ingredientId}.");
            }

            _logger.LogInformation("Ingredient with ID {IngredientId} updated successfully.", ingredientId);
            return result;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid argument during ingredient update.");
            throw;
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
            _logger.LogWarning("Ingredient deletion failed: invalid ID {IngredientId}.", ingredientId);
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