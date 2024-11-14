using FreshInventory.Application.DTO;
using FreshInventory.Application.Interfaces;
using FreshInventory.Application.CQRS.Commands.CreateRecipe;
using FreshInventory.Application.CQRS.Commands.UpdateRecipe;
using FreshInventory.Application.CQRS.Commands.DeleteRecipe;
using FreshInventory.Application.CQRS.Commands.ReactivateRecipe;
using FreshInventory.Application.CQRS.Commands.ReserveIngredients;
using MediatR;
using FreshInventory.Domain.Interfaces;
using FreshInventory.Application.Common;

namespace FreshInventory.Application.Services
{
    public class RecipeService(IRecipeRepository recipeRepository, IMediator mediator) : IRecipeService
    {
        private readonly IMediator _mediator = mediator;
        private readonly IRecipeRepository _recipeRepository = recipeRepository;

        public async Task<PagedList<RecipeDto>> GetAllRecipesAsync(int pageNumber, int pageSize, string? name = null, string? sortBy = null, string? sortDirection = null)
        {
            var (recipes, totalCount) = await _recipeRepository.GetAllRecipesAsync(pageNumber, pageSize, name, sortBy, sortDirection);
            var recipeDtos = recipes.Select(r => new RecipeDto { Id = r.Id, Name = r.Name }).ToList(); // Simplificação para o exemplo
            return new PagedList<RecipeDto>(recipeDtos, totalCount, pageNumber, pageSize);
        }

        public async Task<RecipeDto> CreateRecipeAsync(RecipeCreateDto recipeCreateDto)
        {
            var command = new CreateRecipeCommand(recipeCreateDto);
            return await _mediator.Send(command);
        }

        public async Task<RecipeDto> UpdateRecipeAsync(RecipeUpdateDto recipeUpdateDto)
        {
            var command = new UpdateRecipeCommand(recipeUpdateDto);
            return await _mediator.Send(command);
        }

        public async Task DeleteRecipeAsync(int recipeId)
        {
            var command = new SoftDeleteRecipeCommand(recipeId);
            await _mediator.Send(command);
        }

        public async Task ReactivateRecipeAsync(int recipeId)
        {
            var command = new ReactivateRecipeCommand(recipeId);
            await _mediator.Send(command);
        }

        public async Task<bool> ReserveIngredientsAsync(int recipeId)
        {
            var command = new ReserveIngredientsForRecipeCommand(recipeId);
            return await _mediator.Send(command);
        }

        public async Task<RecipeDto> GetRecipeByIdAsync(int recipeId)
        {
            return (RecipeDto)await _mediator.Send(new object());
        }
    }
}
