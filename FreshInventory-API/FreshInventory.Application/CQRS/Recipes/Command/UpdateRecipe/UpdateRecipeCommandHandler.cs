using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.DTO;
using FreshInventory.Application.Exceptions;
using MediatR;

namespace FreshInventory.Application.CQRS.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, RecipeDto>
    {
        private readonly IRecipeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateRecipeCommandHandler(IRecipeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<RecipeDto> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var existingRecipe = await _repository.GetByIdAsync(request.RecipeUpdateDto.Id)
                ?? throw new ServiceException($"Recipe with ID {request.RecipeUpdateDto.Id} not found.");

            _mapper.Map(request.RecipeUpdateDto, existingRecipe);
            await _repository.UpdateAsync(existingRecipe);

            return _mapper.Map<RecipeDto>(existingRecipe);
        }
    }
}