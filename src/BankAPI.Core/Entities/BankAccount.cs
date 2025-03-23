using BankAPI.Core.Enums;

namespace BankAPI.Core.Entities
{
    public class BankAccount : BaseEntity
    {
        public const decimal BONIFICACAO_INICIAL = 1000m;
        
        public BankAccount(string name, string document)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome não pode ser vazio ou nulo.", nameof(name));

            if (string.IsNullOrWhiteSpace(document))
                throw new ArgumentException("O documento não pode ser vazio ou nulo.", nameof(document));

            Name = name;
            Document = document;
            Balance = BONIFICACAO_INICIAL;
            OpeningDate = DateTime.Now;
            Status = BankAccountStatus.Active;
        }

        public string Name { get; private set; }

        public string Document { get; private set; }

        public decimal Balance { get; private set; }

        public DateTime OpeningDate { get; private set; }

        public BankAccountStatus Status { get; private set; }
    }
}
