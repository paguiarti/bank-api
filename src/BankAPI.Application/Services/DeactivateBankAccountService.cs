using BankAPI.Application.Dtos;
using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Dtos.ViewModels;
using BankAPI.Application.Interfaces.Services;
using BankAPI.Core.Exceptions;
using BankAPI.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BankAPI.Application.Services
{
    public class DeactivateBankAccountService : IDeactivateBankAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeactivateBankAccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomApiResponse<DeactivateBankAccountViewModel>> DeactivateBankAccountAsync(DeactivateBankAccountInputModel inputModel)
        {
            var bankAccount = await _unitOfWork.BankAccountRepository.GetByDocumentAsync(inputModel.Document);
            if (bankAccount == null)
            {
                return CustomApiResponse<DeactivateBankAccountViewModel>.FailResponse(
                    "Conta bancária não encontrada.",
                    StatusCodes.Status400BadRequest);
            }

            try
            {
                bankAccount.DeactivateAccount(inputModel.User);

                _unitOfWork.BankAccountRepository.Update(bankAccount);
                
                await _unitOfWork.SaveChangesAsync();

                return CustomApiResponse<DeactivateBankAccountViewModel>.SuccessResponse(
                    new DeactivateBankAccountViewModel(bankAccount.Id, bankAccount.Document, bankAccount.Status.ToString()),
                    "Conta bancária inativada com sucesso.");
            }
            catch (InvalidBankAccountStatusException ex)
            {
                return CustomApiResponse<DeactivateBankAccountViewModel>.FailResponse(
                    ex.Message,
                    StatusCodes.Status400BadRequest);
            }
        }
    }
}
