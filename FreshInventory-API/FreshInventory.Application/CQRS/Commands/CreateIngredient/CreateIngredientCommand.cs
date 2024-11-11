﻿using FreshInventory.Application.DTO;
using MediatR;

namespace FreshInventory.Application.CQRS.Commands.CreateIngredient;

public class CreateIngredientCommand(IngredientCreateDto ingredientCreateDto) : IRequest<IngredientDto>
{
    public IngredientCreateDto IngredientCreateDto { get; } = ingredientCreateDto;
}