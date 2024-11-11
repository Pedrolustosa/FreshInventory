﻿using FluentValidation;
using FreshInventory.Application.DTO;

namespace FreshInventory.Application.Validators;

public class IngredientUpdateDtoValidator : AbstractValidator<IngredientUpdateDto>
{
    public IngredientUpdateDtoValidator()
    {
        RuleFor(i => i.Id)
            .GreaterThan(0).WithMessage("ID is required for update and must be greater than zero.");

        RuleFor(i => i.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(2, 100).WithMessage("Name must be between 2 and 100 characters.");

        RuleFor(i => i.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(i => i.Unit)
            .IsInEnum().WithMessage("Unit must be a valid value.");

        RuleFor(i => i.UnitCost)
            .GreaterThan(0).WithMessage("Unit cost must be greater than zero.");

        RuleFor(i => i.Category)
            .IsInEnum().WithMessage("Category must be a valid value.");

        RuleFor(i => i.Supplier)
            .NotEmpty().WithMessage("Supplier is required.")
            .Length(2, 50).WithMessage("Supplier must be between 2 and 50 characters.");

        RuleFor(i => i.PurchaseDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Purchase date cannot be in the future.");

        RuleFor(i => i.ExpiryDate)
            .GreaterThan(i => i.PurchaseDate).WithMessage("Expiry date must be after the purchase date.")
            .When(i => i.IsPerishable).WithMessage("Expiry date is required for perishable items.");

        RuleFor(i => i.ReorderLevel)
            .GreaterThanOrEqualTo(0).WithMessage("Reorder level must be zero or higher.");
    }
}