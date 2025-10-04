using FluentValidation;
using Platform.Domain.DTOs.Auth;

namespace Platform.Application.Core.Auth.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("El formato del email no es válido")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Name));

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("El teléfono no puede exceder los 20 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Address)
                .MaximumLength(200).WithMessage("La dirección no puede exceder los 200 caracteres")
                .When(x => !string.IsNullOrEmpty(x.Address));
        }
    }
}