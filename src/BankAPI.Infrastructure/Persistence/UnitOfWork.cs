using BankAPI.Core.Interfaces;
using BankAPI.Core.Interfaces.Repositories;
using BankAPI.Infrastructure.Persistence.Context;
using BankAPI.Infrastructure.Persistence.Repositories;

namespace BankAPI.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankContext _context;
        private readonly IResilientOperation _resilientOperation;

        public UnitOfWork(
            BankContext context,
            IResilientOperation resilientOperation)
        {
            _context = context;
            _resilientOperation = resilientOperation;
        }

        private IBankAccountRepository? _bankAccountRepository;
        public IBankAccountRepository BankAccountRepository
        {
            get { return _bankAccountRepository ?? (_bankAccountRepository = new BankAccountRepository(_context)); }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollBackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task ExecuteResilientlyAsync(Func<Task> operation)
        { 
            await _resilientOperation.ExecuteAsync(operation);
        }
    }
}
