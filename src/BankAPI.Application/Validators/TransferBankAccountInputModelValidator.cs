using BankAPI.Application.Dtos.InputModels;
using FluentValidation;

namespace BankAPI.Application.Validators
{
    public class TransferBankAccountInputModelValidator : AbstractValidator<TransferBankAccountInputModel>
    {
        public TransferBankAccountInputModelValidator()
        {
            RuleFor(x => x.BankAccountIdFrom)
                .NotEmpty()
                .WithMessage("'account_number_from' é obrigatória.");

            RuleFor(x => x.BankAccountIdTo)
                .NotEmpty()
                .WithMessage("account_number_to é obrigatória.");

            RuleFor(x => x.Amount)
                .NotEmpty()
                .WithMessage("'amount' é obrigatório.")
                .GreaterThan(0)
                .WithMessage("'amount' deve ser maior do que zero.");
        }
    }
}
