using AutoMapper;
using FreshInventory.Application.CQRS.Ingredient.Commands;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Ingredients.Handlers
{
    public class CreateIngredientCommandHandler(
        IIngredientRepository ingredientRepository,
        ISupplierRepository supplierRepository,
        IMapper mapper,
        ILogger<CreateIngredientCommandHandler> logger) : IRequestHandler<CreateIngredientCommand, IngredientReadDto>
    {
        private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateIngredientCommandHandler> _logger = logger;

        public async Task<IngredientReadDto> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Received null IngredientCreateDto in CreateIngredientCommandHandler.");
                throw new ArgumentNullException(nameof(request), "IngredientCreateDto cannot be null.");
            }

            try
            {
                _logger.LogInformation("Validating supplier with ID {SupplierId}.", request.SupplierId);
                var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);

                if (supplier == null)
                {
                    _logger.LogWarning("Supplier with ID {SupplierId} not found.", request.SupplierId);
                    throw new ArgumentException($"Supplier with ID {request.SupplierId} does not exist.");
                }

                _logger.LogInformation("Creating Ingredient entity.");
                var ingredient = new Ingredient(request.Name, request.Quantity, request.UnitCost, supplier);

                _logger.LogInformation("Adding ingredient {IngredientName} to the repository.", ingredient.Name);
                await _ingredientRepository.AddIngredientAsync(ingredient);

                _logger.LogInformation("Ingredient {IngredientName} created successfully with ID {IngredientId}.", ingredient.Name, ingredient.Id);

                return _mapper.Map<IngredientReadDto>(ingredient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating ingredient {IngredientName}.", request?.Name);
                throw;
            }
        }
    }
}
