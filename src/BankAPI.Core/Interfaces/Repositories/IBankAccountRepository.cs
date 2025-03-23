using BankAPI.Core.Entities;

namespace BankAPI.Core.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> AddAsync(BankAccount bankAccount);

        Task<BankAccount> GetByDocumentAsync(string document);

        Task<bool> ExistsByDocumentAsync(string document);
    }
}
