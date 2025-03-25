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

            RuleFor(x => x.BankAccountId)
                .NotEmpty().WithMessage("'account_number' é obrigatório.");                
        }
    }
}
