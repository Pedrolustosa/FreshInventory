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
            try
            {
                var recipe = _mapper.Map<Recipe>(request.RecipeCreateDto);
                await _repository.AddAsync(recipe);

                _logger.LogInformation("Recipe '{Name}' created successfully.", recipe.Name);

                return _mapper.Map<RecipeDto>(recipe);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating recipe '{Name}'.", request.RecipeCreateDto.Name);
                throw new ServiceException("An unexpected error occurred while creating the recipe.", ex);
            }
        }
    }
}
