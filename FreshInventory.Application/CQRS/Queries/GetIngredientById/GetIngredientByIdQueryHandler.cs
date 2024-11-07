using MediatR;
using AutoMapper;
using FreshInventory.Application.DTO;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;

namespace FreshInventory.Application.CQRS.Queries.GetIngredientById;

public class GetIngredientByIdQueryHandler(IIngredientRepository repository, IMapper mapper) : IRequestHandler<GetIngredientByIdQuery, IngredientDto>
{
    private readonly IMapper _mapper = mapper;
    private readonly IIngredientRepository _repository = repository;

    public async Task<IngredientDto> Handle(GetIngredientByIdQuery request, CancellationToken cancellationToken)
    {
        var ingredient = await _repository.GetByIdAsync(request.Id) ?? throw new ServiceException($"Ingredient with ID {request.Id} not found.");
        var ingredientDto = _mapper.Map<IngredientDto>(ingredient);
        return ingredientDto;
    }
}
