using BankAPI.Application.Dtos.InputModels;
using FluentValidation;

namespace BankAPI.Application.Validators
{
    public class AddBankAccountInputModelValidator : AbstractValidator<AddBankAccountInputModel>
    {
        public AddBankAccountInputModelValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("'name' é obrigatório.")
            .Length(5, 200).WithMessage("'name' deve ter entre 3 e 200 caracteres.");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("'document' é obrigatório.")
                .Matches(@"^\d+$").WithMessage("'document' deve conter apenas números.")
                .Length(11, 14).WithMessage("'document' deve ter entre 11 e 14 caracteres.");
        }
    }
}
