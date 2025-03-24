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

        public async Task<int> CountAsync(string name = "", string document = "")
        {
            var query = _context.BankAccounts.AsQueryable();

            query = ApplyFilters(query, name, document);

            return await query.CountAsync();
        }

        public async Task<bool> ExistsByDocumentAsync(string document)
        {
            return await _context
                            .BankAccounts
                            .Where(b => b.Document == document)
                            .AnyAsync();
        }

        public async Task<IEnumerable<BankAccount>> GetAllAsync(string name = "", string document = "", int page = 1, int pageSize = 10)
        {
            var query = _context.BankAccounts.AsQueryable();

            query = ApplyFilters(query, name, document);

            query = query
                .OrderBy(x => x.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<BankAccount?> GetByDocumentAsync(string document)
        {
            return await _context
                            .BankAccounts
                            .Where(b => b.Document == document)
                            .FirstOrDefaultAsync();
        }

        public void Update(BankAccount bankAccount)
        {
            _context.BankAccounts.Update(bankAccount);            
        }

        private IQueryable<BankAccount> ApplyFilters(
            IQueryable<BankAccount> query,
            string name = "",
            string document = "")
        {
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(b => b.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(document))
            {
                query = query.Where(b => b.Document == document);
            }

            return query;
        }
    }
}
