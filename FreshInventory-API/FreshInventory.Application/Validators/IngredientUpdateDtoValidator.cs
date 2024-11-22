using FluentValidation;
using FreshInventory.Application.DTO.IngredientDTO;

namespace FreshInventory.Application.Validators;

public class IngredientUpdateDtoValidator : AbstractValidator<IngredientUpdateDto>
{
    public IngredientUpdateDtoValidator()
    {
        RuleFor(i => i.Id);

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

        RuleFor(i => i.SupplierId.ToString())
            .NotEmpty().WithMessage("Supplier is required.");

        RuleFor(i => i.PurchaseDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Purchase date cannot be in the future.");

        RuleFor(i => i.ExpiryDate)
            .GreaterThan(i => i.PurchaseDate).WithMessage("Expiry date must be after the purchase date.")
            .When(i => i.IsPerishable).WithMessage("Expiry date is required for perishable items.");

        RuleFor(i => i.ReorderLevel)
            .GreaterThanOrEqualTo(0).WithMessage("Reorder level must be zero or higher.");
    }
}
