using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, RecipeDto>
    {
        private readonly IRecipeRepository _repository;
        private readonly IMapper _mapper;

        public CreateRecipeCommandHandler(IRecipeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RecipeDto> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = _mapper.Map<Recipe>(request.RecipeCreateDto);
            await _repository.AddAsync(recipe);
            return _mapper.Map<RecipeDto>(recipe);
        }
    }
}