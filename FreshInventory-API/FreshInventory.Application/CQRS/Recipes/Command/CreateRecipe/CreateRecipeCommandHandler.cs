using AutoMapper;
using FreshInventory.Domain.Entities;
using FreshInventory.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.Exceptions;
using FreshInventory.Application.DTO.RecipeDTO;

namespace FreshInventory.Application.CQRS.Commands.CreateRecipe
{
    public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, RecipeDto>
    {
        private readonly IRecipeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRecipeCommandHandler> _logger;

        public CreateRecipeCommandHandler(IRecipeRepository repository, IMapper mapper, ILogger<CreateRecipeCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RecipeDto> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
        {
            if (request.Ingredients == null || request.Ingredients.Count == 0)
            {
                throw new Exception("Recipe must have at least one ingredient.");
            }

            var recipe = _mapper.Map<Recipe>(request);
            await _repository.AddAsync(recipe);
            return _mapper.Map<RecipeDto>(recipe);
        }
    }
}
