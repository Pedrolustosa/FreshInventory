using AutoMapper;
using FreshInventory.Application.CQRS.Ingredient.Commands;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Application.Features.Ingredients.Commands;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using MediatR;

namespace FreshInventory.Application.Features.Ingredients.Handlers
{
    public class CreateIngredientCommandHandler : IRequestHandler<CreateIngredientCommand, IngredientReadDto>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public CreateIngredientCommandHandler(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<IngredientReadDto> Handle(CreateIngredientCommand request, CancellationToken cancellationToken)
        {
            var ingredient = _mapper.Map<Ingredient>(request.IngredientCreateDto);
            await _ingredientRepository.AddIngredientAsync(ingredient);
            return _mapper.Map<IngredientReadDto>(ingredient);
        }
    }
}
