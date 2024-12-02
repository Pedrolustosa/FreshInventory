using AutoMapper;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Domain.Exceptions;
using MediatR;
using FreshInventory.Application.Features.Ingredients.Queries;

namespace FreshInventory.Application.Features.Ingredients.Handlers
{
    public class GetIngredientByIdQueryHandler : IRequestHandler<GetIngredientByIdQuery, IngredientReadDto>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public GetIngredientByIdQueryHandler(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<IngredientReadDto> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
        {
            var ingredient = await _ingredientRepository.GetIngredientByIdAsync(request.IngredientId);
            if (ingredient == null)
            {
                throw new RepositoryException($"Ingredient with ID {request.IngredientId} not found.");
            }

            return _mapper.Map<IngredientReadDto>(ingredient);
        }
    }
}
