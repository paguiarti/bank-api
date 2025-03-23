using BankAPI.Core.Entities;
using BankAPI.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Infrastructure.Persistence.Context
{
    public class BankContext : DbContext
    {
        public BankContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<BankAccount> BankAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BankAccountConfiguration).Assembly);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
