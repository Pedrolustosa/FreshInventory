using AutoMapper;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using FreshInventory.Application.DTO.RecipeDTO;

namespace FreshInventory.Application.CQRS.Commands.UpdateRecipe
{
    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, RecipeDto>
    {
        private readonly IRecipeRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateRecipeCommandHandler> _logger;

        public UpdateRecipeCommandHandler(IRecipeRepository repository, IMapper mapper, ILogger<UpdateRecipeCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RecipeDto> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingRecipe = await _repository.GetByIdAsync(request.Id)
                    ?? throw new ServiceException($"Recipe with ID {request.Id} not found.");

                _mapper.Map(request, existingRecipe);
                await _repository.UpdateAsync(existingRecipe);

                _logger.LogInformation("Recipe with ID {RecipeId} updated successfully.", request.Id);

                return _mapper.Map<RecipeDto>(existingRecipe);
            }
            catch (ServiceException ex)
            {
                _logger.LogError(ex, "Service error while updating recipe with ID {RecipeId}.", request.Id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while updating recipe with ID {RecipeId}.", request.Id);
                throw new Exception("An unexpected error occurred while updating the recipe.", ex);
            }
        }
    }
}
