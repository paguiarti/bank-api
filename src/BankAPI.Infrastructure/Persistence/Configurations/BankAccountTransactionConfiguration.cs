using BankAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAPI.Infrastructure.Persistence.Configurations
{
    public class BankAccountTransactionConfiguration : IEntityTypeConfiguration<BankAccountTransaction>
    {
        public void Configure(EntityTypeBuilder<BankAccountTransaction> builder)
        {
            builder.ToTable("BankAccountTransactions");

            builder
                .Property(b => b.TransactionType)
                .HasConversion<string>()
                .HasMaxLength(100);
        }
    }
}
