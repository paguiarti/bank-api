using BankAPI.Application.Dtos.InputModels;
using FluentValidation;

namespace BankAPI.Application.Validators
{
    public class DeactivateBankAccountInputModelValidator : AbstractValidator<DeactivateBankAccountInputModel>
    {
        public DeactivateBankAccountInputModelValidator()
        {
            RuleFor(x => x.User)
            .NotEmpty().WithMessage("'user' é obrigatório.");            

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("'document' é obrigatório.")
                .Matches(@"^\d+$").WithMessage("'document' deve conter apenas números.")
                .Length(11, 14).WithMessage("'document' deve ter entre 11 e 14 caracteres.");
        }
    }
}
