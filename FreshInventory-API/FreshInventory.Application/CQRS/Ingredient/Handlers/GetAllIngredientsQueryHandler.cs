using MediatR;
using AutoMapper;
using FreshInventory.Application.DTO.IngredientDTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Features.Ingredients.Queries;

namespace FreshInventory.Application.Features.Ingredients.Handlers
{
    public class GetAllIngredientsQueryHandler : IRequestHandler<GetAllIngredientsQuery, IEnumerable<IngredientReadDto>>
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public GetAllIngredientsQueryHandler(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IngredientReadDto>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
        {
            var ingredients = await _ingredientRepository.GetAllIngredientsAsync();
            return _mapper.Map<IEnumerable<IngredientReadDto>>(ingredients);
        }
    }
}
