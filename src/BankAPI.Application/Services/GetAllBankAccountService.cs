using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Dtos.ViewModels;
using BankAPI.Application.Interfaces.Services;
using BankAPI.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BankAPI.Application.Services
{
    public class GetAllBankAccountService : IGetAllBankAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllBankAccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginationApiResponse<IEnumerable<BankAccountViewModel>>> GetAllAsync(GetAllBankAccountInputModel inputModel)
        {
            var bankAccounts = await _unitOfWork
                                        .BankAccountRepository
                                        .GetAllAsync(
                                            inputModel.Name,
                                            inputModel.Document,
                                            inputModel.Page,
                                            inputModel.PageSize);

            var totalCount = await _unitOfWork.BankAccountRepository.CountAsync(inputModel.Name, inputModel.Document);
            var totalPages = (int)Math.Ceiling((double)totalCount / inputModel.PageSize);

            if (inputModel.Page > totalPages)
            {
                inputModel.Page = 1;
            }

            var viewModels = bankAccounts.Select(BankAccountViewModel.FromEntity).ToList();

            return PaginationApiResponse<IEnumerable<BankAccountViewModel>>.SuccessResponse(
                viewModels, 
                StatusCodes.Status200OK, 
                inputModel.Page, 
                inputModel.PageSize, 
                totalCount);
        }
    }
}
