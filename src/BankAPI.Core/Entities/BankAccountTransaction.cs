using BankAPI.Core.Enums;

namespace BankAPI.Core.Entities
{
    public class BankAccountTransaction : BaseEntity
    {
        public BankAccountTransaction(
            int bankAccountId, 
            decimal amount, 
            TransactionType transactionType, 
            string description,            
            Guid? transactionPairId)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("O valor da transação deve ser maior que zero.", nameof(amount));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("Descrição da transação deve ser informada.", nameof(description));
            }

            BankAccountId = bankAccountId;            
            Amount = amount;
            TransactionType = transactionType;
            Description = description;            
            TransactionPairId = transactionPairId;
            TransactionDate = DateTime.UtcNow;
        }

        public int BankAccountId { get; private set; }

        public DateTime TransactionDate { get; private set; }

        public decimal Amount { get; private set; }

        public TransactionType TransactionType { get; private set; }

        public string Description { get; private set; }

        public Guid? TransactionPairId { get; set; }
    }
}
