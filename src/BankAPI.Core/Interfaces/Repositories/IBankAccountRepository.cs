using BankAPI.Core.Entities;

namespace BankAPI.Core.Interfaces.Repositories
{
    public interface IBankAccountRepository
    {
        Task<BankAccount> AddAsync(BankAccount bankAccount);

        Task<BankAccount> GetByDocumentAsync(string document);

        Task<bool> ExistsByDocumentAsync(string document);

        Task<IEnumerable<BankAccount>> GetAllAsync(string name = "", string document = "", int page = 1, int pageSize = 10);

        Task<int> CountAsync(string name = "", string document = "");
    }
}
