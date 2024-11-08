using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Queries.GetIngredientById;

public class GetIngredientByIdQueryHandler(
    IIngredientRepository repository,
    IMapper mapper,
    ILogger<GetIngredientByIdQueryHandler> logger) : IRequestHandler<GetIngredientByIdQuery, IngredientDto>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<GetIngredientByIdQueryHandler> _logger = logger;

    public async Task<IngredientDto> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ingredient = await _repository.GetByIdAsync(request.Id);

            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {Id} not found.", request.Id);
                throw new QueryException($"Ingredient with ID {request.Id} not found.");
            }

            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);

            _logger.LogInformation("Ingredient with ID {Id} retrieved successfully.", request.Id);

            return ingredientDto;
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving ingredient with ID {Id}.", request.Id);
            throw new QueryException("An error occurred while retrieving the ingredient.", ex);
        }
        catch (QueryException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while retrieving ingredient with ID {Id}.", request.Id);
            throw new QueryException("An unexpected error occurred while retrieving the ingredient.", ex);
        }
    }
}
