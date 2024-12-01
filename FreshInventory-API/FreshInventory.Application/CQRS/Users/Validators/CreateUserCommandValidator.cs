using FluentValidation;
using FreshInventory.Application.Features.Users.Commands;

namespace FreshInventory.Application.Features.Users.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Nome completo é obrigatório.")
                .MaximumLength(100).WithMessage("Nome completo não pode exceder 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("As senhas não coincidem.");

            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.UtcNow).WithMessage("Data de nascimento deve ser no passado.");
        }
    }
}
