using BankAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAPI.Infrastructure.Persistence.Configurations
{
    public class BankAccountConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.ToTable("BankAccounts");

            builder
                .Property(b => b.Name)
                .HasMaxLength(200);

            builder
                .Property(b => b.Document)
                .HasMaxLength(20);

            builder
                .HasIndex(b => b.Document)
                .IsUnique();
                
        }
    }
}
