using BankAPI.Application.Dtos.InputModels;
using FluentValidation;

namespace BankAPI.Application.Validators
{
    public class AddBankAccountInputModelValidator : AbstractValidator<AddBankAccountInputModel>
    {
        public AddBankAccountInputModelValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .Length(5, 200).WithMessage("O nome deve ter entre 3 e 200 caracteres.");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("O documento é obrigatório.")
                .Matches(@"^\d+$").WithMessage("O documento deve conter apenas números.")
                .Length(11, 14).WithMessage("O documento deve ter entre 11 e 14 caracteres.");
        }
    }
}
