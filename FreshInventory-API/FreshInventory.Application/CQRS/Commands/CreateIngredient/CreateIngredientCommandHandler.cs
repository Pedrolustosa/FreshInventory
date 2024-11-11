using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Domain.Interfaces;

namespace FreshInventory.Application.CQRS.Commands.CreateIngredient;

public class CreateIngredientCommandHandler(
    IIngredientRepository repository,
    IMapper mapper,
    ILogger<CreateIngredientCommandHandler> logger) : IRequestHandler<CreateIngredientCommand, IngredientDto>
{
    private readonly IIngredientRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CreateIngredientCommandHandler> _logger = logger;

    public async Task<IngredientDto> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var ingredient = _mapper.Map<Ingredient>(request.IngredientCreateDto);
            await _repository.AddAsync(ingredient);
            var ingredientDto = _mapper.Map<IngredientDto>(ingredient);

            _logger.LogInformation("Ingredient '{Name}' created successfully.", ingredient.Name);

            return ingredientDto;
        }
        catch (RepositoryException ex)
        {
            _logger.LogError(ex, "An error occurred while creating ingredient '{Name}'.", request.IngredientCreateDto.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating ingredient '{Name}'.", request.IngredientCreateDto.Name);
            throw new Exception("An unexpected error occurred while creating the ingredient.", ex);
        }
    }
}
