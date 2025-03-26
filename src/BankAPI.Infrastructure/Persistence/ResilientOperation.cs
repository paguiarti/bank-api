using BankAPI.Core.Interfaces;
using BankAPI.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Infrastructure.Persistence
{
    public class ResilientOperation : IResilientOperation
    {
        private readonly BankContext _context;

        public ResilientOperation(BankContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync(Func<Task> operation)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(operation);
        }
    }
}
