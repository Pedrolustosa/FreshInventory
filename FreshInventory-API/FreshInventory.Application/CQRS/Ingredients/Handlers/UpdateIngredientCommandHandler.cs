using AutoMapper;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FreshInventory.Application.Features.Ingredients.Handlers
{
    public class UpdateIngredientCommandHandler(
        IIngredientRepository ingredientRepository,
        ISupplierRepository supplierRepository,
        IMapper mapper,
        ILogger<UpdateIngredientCommandHandler> logger) : IRequestHandler<UpdateIngredientCommand, IngredientReadDto>
    {
        private readonly IIngredientRepository _ingredientRepository = ingredientRepository;
        private readonly ISupplierRepository _supplierRepository = supplierRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UpdateIngredientCommandHandler> _logger = logger;

        public async Task<IngredientReadDto> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to update ingredient with ID {IngredientId}.", request.IngredientId);

            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(request.IngredientId);
            if (ingredient == null)
            {
                _logger.LogWarning("Ingredient with ID {IngredientId} not found.", request.IngredientId);
                throw new InvalidOperationException($"Ingredient with ID {request.IngredientId} not found.");
            }

            var supplier = await _supplierRepository.GetByIdAsync(request.IngredientUpdateDto.SupplierId);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier with ID {SupplierId} not found.", request.IngredientUpdateDto.SupplierId);
                throw new ArgumentException($"Supplier with ID {request.IngredientUpdateDto.SupplierId} does not exist.");
            }

            _logger.LogInformation("Updating Ingredient entity.");
            ingredient.Update(
                request.IngredientUpdateDto.Name,
                request.IngredientUpdateDto.Quantity,
                request.IngredientUpdateDto.UnitCost,
                supplier);

            await _ingredientRepository.UpdateIngredientAsync(ingredient);

            _logger.LogInformation("Ingredient with ID {IngredientId} updated successfully.", request.IngredientId);

            return _mapper.Map<IngredientReadDto>(ingredient);
        }
    }
}
