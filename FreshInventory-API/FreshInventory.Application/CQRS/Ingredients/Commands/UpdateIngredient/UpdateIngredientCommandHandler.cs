using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Ingredients.Commands.UpdateIngredient;

public class UpdateIngredientCommandHandler(
    IIngredientRepository repository,
    IMapper mapper,
    ILogger<UpdateIngredientCommandHandler> logger) : IRequestHandler<UpdateIngredientCommand>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<UpdateIngredientCommandHandler> _logger = logger;

    public async Task Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var existingIngredient = await _repository.GetByIdAsync(request.IngredientUpdateDto.Id)
                ?? throw new ServiceException($"Ingredient with ID {request.IngredientUpdateDto.Id} not found.");

            _mapper.Map(request.IngredientUpdateDto, existingIngredient);
            await _repository.UpdateAsync(existingIngredient);

            _logger.LogInformation("Ingredient '{Name}' updated successfully.", existingIngredient.Name);
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "An error occurred while updating ingredient '{Name}'.", request.IngredientUpdateDto.Name);
            throw;
        }
        catch (ServiceException ex)
        {
            _logger.LogError(ex, "Service error while updating ingredient '{Name}'.", request.IngredientUpdateDto.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while updating ingredient '{Name}'.", request.IngredientUpdateDto.Name);
            throw new Exception("An unexpected error occurred while updating the ingredient.", ex);
        }
    }
}
