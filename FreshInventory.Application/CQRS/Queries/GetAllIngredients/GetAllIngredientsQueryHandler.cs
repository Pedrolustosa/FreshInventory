using MediatR;
using AutoMapper;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Interfaces;

namespace FreshInventory.Application.CQRS.Queries.GetAllIngredients;

public class GetAllIngredientsQueryHandler(IIngredientRepository repository, IMapper mapper) : IRequestHandler<GetAllIngredientsQuery, IEnumerable<IngredientDto>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IIngredientRepository _repository = repository;

    public async Task<IEnumerable<IngredientDto>> Handle(GetAllIngredientsQuery request, CancellationToken cancellationToken)
    {
        var ingredients = await _repository.GetAllAsync();
        var ingredientDtos = _mapper.Map<IEnumerable<IngredientDto>>(ingredients);
        return ingredientDtos;
    }
}
