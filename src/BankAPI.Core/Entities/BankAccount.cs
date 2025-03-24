using BankAPI.Core.Enums;
using BankAPI.Core.Exceptions;

namespace BankAPI.Core.Entities
{
    public class BankAccount : BaseEntity
    {
        public const decimal INITIAL_BONUS = 1000m;

        public BankAccount(string name, string document)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("O nome não pode ser vazio ou nulo.", nameof(name));

            if (string.IsNullOrWhiteSpace(document))
                throw new ArgumentException("O documento não pode ser vazio ou nulo.", nameof(document));

            Name = name;
            Document = document;
            Balance = INITIAL_BONUS;
            OpeningDate = DateTime.Now;
            Status = BankAccountStatus.Active;
            StatusHistory = new List<BankAccountStatusHistory>();
        }

        public string Name { get; private set; }

        public string Document { get; private set; }

        public decimal Balance { get; private set; }

        public DateTime OpeningDate { get; private set; }

        public BankAccountStatus Status { get; private set; }

        public List<BankAccountStatusHistory> StatusHistory { get; private set; }

        public void DeactivateAccount(string user)
        {
            if (!Status.Equals(BankAccountStatus.Active))
            {
                throw new InvalidBankAccountStatusException($"Conta bancária não se encontra ativa para ser desativada.");
            }

            Status = BankAccountStatus.Inactive;

            StatusHistory.Add(new BankAccountStatusHistory(
                Id,
                Status,
                user
                ));
        }
    }
}
