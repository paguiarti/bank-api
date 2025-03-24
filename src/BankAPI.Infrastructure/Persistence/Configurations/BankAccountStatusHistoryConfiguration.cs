using BankAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAPI.Infrastructure.Persistence.Configurations
{
    public class BankAccountStatusHistoryConfiguration : IEntityTypeConfiguration<BankAccountStatusHistory>
    {
        public void Configure(EntityTypeBuilder<BankAccountStatusHistory> builder)
        {
            builder.ToTable("BankAccountStatusHistory");

            builder
                .Property(b => b.User)
                .HasMaxLength(100);
        }
    }
}
