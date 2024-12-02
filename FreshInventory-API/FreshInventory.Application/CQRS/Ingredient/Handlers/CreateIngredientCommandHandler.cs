using AutoMapper;
using FreshInventory.Application.CQRS.Ingredient.Commands;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Ingredients.Handlers;

public class CreateIngredientCommandHandler(
    IIngredientRepository ingredientRepository,
    IMapper mapper,
    ILogger<CreateIngredientCommandHandler> logger) : IRequestHandler<CreateIngredientCommand, IngredientReadDto>
{
    private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<CreateIngredientCommandHandler> _logger = logger;

    public async Task<IngredientReadDto> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
    {
        if (request.IngredientCreateDto == null)
        {
            _logger.LogWarning("CreateIngredientCommandHandler received null data.");
            throw new ArgumentNullException(nameof(request.IngredientCreateDto), "IngredientCreateDto cannot be null.");
        }

        try
        {
            _logger.LogInformation("Mapping IngredientCreateDto to Ingredient entity.");
            var ingredient = _mapper.Map<Ingredient>(request.IngredientCreateDto);

            _logger.LogInformation("Adding ingredient {IngredientName} to the repository.", ingredient.Name);
            await _ingredientRepository.AddIngredientAsync(ingredient);

            _logger.LogInformation("Ingredient {IngredientName} created successfully with ID {IngredientId}.", ingredient.Name, ingredient.Id);
            return _mapper.Map<IngredientReadDto>(ingredient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the ingredient.");
            throw;
        }
    }
}
