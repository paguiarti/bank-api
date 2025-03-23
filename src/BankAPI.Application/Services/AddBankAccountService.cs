using BankAPI.Application.Dtos;
using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Dtos.ViewModels;
using BankAPI.Application.Interfaces.Services;
using BankAPI.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BankAPI.Application.Services
{
    public class AddBankAccountService : IAddBankAccountService
    {
        public const string ACCOUNT_ALREADY_EXISTS_MSG = "Conta já cadastrada para o documento informado.";
        public const string ACCOUNT_CREATED_MSG = "Conta criada com sucesso.";
        private readonly IUnitOfWork _unitOfWork;

        public AddBankAccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomApiResponse<AddBankAccountViewModel>> AddAsync(AddBankAccountInputModel inputModel)
        {
            var accountExists = await _unitOfWork.BankAccountRepository.ExistsByDocumentAsync(inputModel.Document);
            if (accountExists)
            {
                return CustomApiResponse<AddBankAccountViewModel>.FailResponse(
                    ACCOUNT_ALREADY_EXISTS_MSG,
                    StatusCodes.Status409Conflict);
            }

            var bankAccountEntity = await _unitOfWork.BankAccountRepository.AddAsync(inputModel.ToEntity());
            await _unitOfWork.SaveChangesAsync();

            return CustomApiResponse<AddBankAccountViewModel>.SuccessResponse(
                AddBankAccountViewModel.FromEntity(bankAccountEntity), 
                ACCOUNT_CREATED_MSG, 
                StatusCodes.Status201Created);
        }
    }
}
