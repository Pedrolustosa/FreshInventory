using MediatR;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Application.Features.Ingredients.Commands;

namespace FreshInventory.Application.Features.Ingredients.Handlers
{
    public class DeleteIngredientCommandHandler : IRequestHandler<DeleteIngredientCommand, bool>
    {
        private readonly IIngredientRepository _ingredientRepository;

        public DeleteIngredientCommandHandler(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public async Task<bool> Handle(DeleteIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(request.IngredientId);
            if (ingredient == null)
            {
                throw new KeyNotFoundException($"Ingredient with ID {request.IngredientId} not found.");
            }

            return await _ingredientRepository.DeleteIngredientAsync(request.IngredientId);
        }

    }
}
