using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Dtos.ViewModels;

namespace BankAPI.Application.Interfaces.Services
{
    public interface IGetAllBankAccountService
    {
        Task<PaginationApiResponse<IEnumerable<BankAccountViewModel>>> GetAllAsync(GetAllBankAccountInputModel inputModel);
    }
}
