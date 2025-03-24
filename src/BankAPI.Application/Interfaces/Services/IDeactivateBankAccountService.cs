using BankAPI.Application.Dtos;
using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Dtos.ViewModels;

namespace BankAPI.Application.Interfaces.Services
{
    public interface IDeactivateBankAccountService
    {
        Task<CustomApiResponse<DeactivateBankAccountViewModel>> DeactivateBankAccountAsync(DeactivateBankAccountInputModel inputModel);
    }
}
