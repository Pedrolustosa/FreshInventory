using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO.IngredientDTO;

namespace FreshInventory.Application.CQRS.Ingredients.Commands.CreateIngredient;

public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, IngredientDto>
{
    private readonly IIngredientRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateIngredientCommandHandler> _logger;

    public CreateIngredientCommandHandler(
        IIngredientRepository repository,
        IMapper mapper,
        ILogger<CreateIngredientCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IngredientDto> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
    {
        if (request?.IngredientCreateDto == null)
        {
            _logger.LogWarning("Received null or invalid data for creating an ingredient.");
            throw new ArgumentException("IngredientCreateDto cannot be null.");
        }

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
            _logger.LogError(ex, "A repository error occurred while creating ingredient '{Name}'.", request.IngredientCreateDto.Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating ingredient '{Name}'.", request.IngredientCreateDto.Name);
            throw new Exception("An unexpected error occurred while creating the ingredient.", ex);
        }
    }
}
