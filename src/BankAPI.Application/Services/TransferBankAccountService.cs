using BankAPI.Application.Dtos;
using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Dtos.ViewModels;
using BankAPI.Application.Interfaces.Services;
using BankAPI.Core.Exceptions;
using BankAPI.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace BankAPI.Application.Services
{
    public class TransferBankAccountService : ITransferBankAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TransferBankAccountService> _logger;

        public TransferBankAccountService(
            IUnitOfWork unitOfWork,
            ILogger<TransferBankAccountService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<CustomApiResponse<TransferBankAccountViewModel>> TransferBankAccountAsync(TransferBankAccountInputModel inputModel)
        {
            var bankAccountFrom = await _unitOfWork
                .BankAccountRepository
                .GetByIdAsync(inputModel.BankAccountIdFrom);
            
            var bankAccountTo = await _unitOfWork
                .BankAccountRepository
                .GetByIdAsync(inputModel.BankAccountIdTo);

            if (bankAccountFrom == null)
            {
                return CustomApiResponse<TransferBankAccountViewModel>.FailResponse("Conta de origem não encontrada.");
            }

            if (bankAccountTo == null)
            {
                return CustomApiResponse<TransferBankAccountViewModel>.FailResponse("Conta de destino não encontrada.");
            }

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                bankAccountFrom.TransferTo(bankAccountTo, inputModel.Amount);

                await _unitOfWork.CommitTransactionAsync();

                return CustomApiResponse<TransferBankAccountViewModel>.SuccessResponse(new TransferBankAccountViewModel(
                    bankAccountFrom.Id,
                    bankAccountTo.Id,
                    inputModel.Amount));

            }
            catch (Exception ex) when (ex is InvalidTransferValueException ||                                
                               ex is InvalidBankAccountStatusException ||
                               ex is InsufficientFundsException ||
                               ex is InvalidTransferDestinationException)
            {
                await _unitOfWork.RollBackTransactionAsync();
                return CustomApiResponse<TransferBankAccountViewModel>.FailResponse(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro inesperado na transferência da conta {bankAccountFrom.Id} para a conta {bankAccountTo.Id}");
                
                await _unitOfWork.RollBackTransactionAsync();                
                return CustomApiResponse<TransferBankAccountViewModel>.FailResponse("Erro ao processar transferência. Tente novamente mais tarde.");
            }
        }
    }
}
