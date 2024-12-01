using FluentValidation;
using FreshInventory.Application.Features.Users.Commands;

namespace FreshInventory.Application.Features.Users.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Nome completo é obrigatório.")
                .Length(3, 150).WithMessage("Nome completo deve ter entre 3 e 150 caracteres.");

            RuleFor(x => x.AlternatePhoneNumber)
                .Matches(@"^\+?\d{10,15}$").WithMessage("Número de telefone inválido.")
                .When(x => !string.IsNullOrEmpty(x.AlternatePhoneNumber));
        }
    }
}
