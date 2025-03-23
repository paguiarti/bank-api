using BankAPI.Core.Entities;
using BankAPI.Core.Interfaces.Repositories;
using BankAPI.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Infrastructure.Persistence.Repositories
{
    public class BankAccountRepository : IBankAccountRepository
    {
        private readonly BankContext _context;

        public BankAccountRepository(BankContext context)
        {
            _context = context;
        }

        public async Task<BankAccount> AddAsync(BankAccount bankAccount)
        {
            await _context.BankAccounts.AddAsync(bankAccount);
            return bankAccount;
        }

        public async Task<bool> ExistsByDocumentAsync(string document)
        {
            return await _context
                            .BankAccounts
                            .Where(b => b.Document == document)
                            .AnyAsync();
        }

        public async Task<BankAccount> GetByDocumentAsync(string document)
        {
            return await _context
                            .BankAccounts
                            .Where(b => b.Document == document)
                            .FirstOrDefaultAsync();
        }
    }
}
