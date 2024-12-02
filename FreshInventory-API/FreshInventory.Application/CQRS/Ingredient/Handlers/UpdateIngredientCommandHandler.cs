using AutoMapper;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Application.Features.Ingredients.Commands;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Exceptions;
using FreshInventory.Domain.Interfaces;
using MediatR;

namespace FreshInventory.Application.Features.Ingredients.Handlers
{
    public class UpdateIngredientCommandHandler : IRequestHandler<UpdateIngredientCommand, IngredientReadDto>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public UpdateIngredientCommandHandler(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<IngredientReadDto> Handle(UpdateIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(request.IngredientId);
            if (ingredient == null)
            {
                throw new RepositoryException($"Ingredient with ID {request.IngredientId} not found.");
            }

            _mapper.Map(request.IngredientUpdateDto, ingredient);
            await _ingredientRepository.UpdateIngredientAsync(ingredient);

            return _mapper.Map<IngredientReadDto>(ingredient);
        }
    }
}
