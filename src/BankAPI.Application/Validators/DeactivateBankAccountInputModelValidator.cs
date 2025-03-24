using BankAPI.Application.Dtos.InputModels;
using FluentValidation;

namespace BankAPI.Application.Validators
{
    public class DeactivateBankAccountInputModelValidator : AbstractValidator<DeactivateBankAccountInputModel>
    {
        public DeactivateBankAccountInputModelValidator()
        {
            RuleFor(x => x.User)
            .NotEmpty().WithMessage("O usuário é obrigatório.");            

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("O documento é obrigatório.")
                .Matches(@"^\d+$").WithMessage("O documento deve conter apenas números.")
                .Length(11, 14).WithMessage("O documento deve ter entre 11 e 14 caracteres.");
        }
    }
}
