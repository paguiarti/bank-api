using BankAPI.Core.Interfaces.Repositories;

namespace BankAPI.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBankAccountRepository BankAccountRepository { get; }

        Task SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollBackTransactionAsync();
        Task ExecuteResilientlyAsync(Func<Task> operation);
    }
}
