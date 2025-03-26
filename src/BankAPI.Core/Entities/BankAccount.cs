using BankAPI.Core.Enums;
using BankAPI.Core.Exceptions;
using System.ComponentModel.DataAnnotations;

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
            OpeningDate = DateTime.UtcNow;
            Status = BankAccountStatus.Active;
            StatusHistory = new List<BankAccountStatusHistory>();
            Transactions = new List<BankAccountTransaction>();
        }

        public string Name { get; private set; }

        public string Document { get; private set; }

        public decimal Balance { get; private set; }

        public DateTime OpeningDate { get; private set; }

        public BankAccountStatus Status { get; private set; }

        [Timestamp]
        public byte[] RowVersion { get; private set; } = null!; // Supressão segura (EF Core garante a inicialização)

        public List<BankAccountStatusHistory> StatusHistory { get; private set; }

        public List<BankAccountTransaction> Transactions { get; private set; }

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

        public void ActivateAccount(string user)
        {
            if (Status.Equals(BankAccountStatus.Active))
            {
                throw new InvalidBankAccountStatusException($"Conta já se encontra ativa.");
            }

            Status = BankAccountStatus.Active;

            StatusHistory.Add(new BankAccountStatusHistory(
                Id,
                Status,
                user
                ));
        }

        public void TransferTo(BankAccount destination, decimal amount)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (amount <= 0)
                throw new InvalidTransferValueException("Valor deve ser maior do que zero.");

            if (Document == destination.Document)
                throw new InvalidTransferDestinationException("Não é possível transferir para a mesma conta.");

            if (Status != BankAccountStatus.Active)
                throw new InvalidBankAccountStatusException("Conta origem deve estar ativa para realizar uma transferência.");

            if (destination.Status != BankAccountStatus.Active)
                throw new InvalidBankAccountStatusException("Conta destino deve estar ativa para receber uma transferência.");

            if (amount > Balance)
                throw new InsufficientFundsException("Saldo insuficiente para realizar a transferência.");

            DecreaseBalance(amount);
            destination.IncreaseBalance(amount);

            var transactionPairId = Guid.NewGuid();

            AddTransaction(new BankAccountTransaction(
                Id,
                amount,
                TransactionType.TransferOut,
                $"Transferência realizada para a conta: {destination.Id}",
                transactionPairId
                ));

            destination.AddTransaction(new BankAccountTransaction(
                Id,
                amount,
                TransactionType.TransferIn,
                $"Transferência recebida da conta: {Id}",
                transactionPairId
                ));
        }

        public void Deposit(decimal amount)
        {
            if (Status != BankAccountStatus.Active)
            {
                throw new InvalidBankAccountStatusException("A conta deve estar ativa para depositar.");
            }

            Balance += amount;

            AddTransaction(new BankAccountTransaction(
                Id,
                amount,
                TransactionType.Deposit,
                $"Depósito realizado",
                null
                ));
        }

        public void Withdrawal(decimal amount)
        {
            if (Status != BankAccountStatus.Active)
            {
                throw new InvalidBankAccountStatusException("A conta deve estar ativa para sacar.");
            }

            if (amount > Balance)
            {
                throw new InsufficientFundsException("Saldo insuficiente para realizar o saque.");
            }

            Balance -= amount;

            AddTransaction(new BankAccountTransaction(
                Id,
                amount,
                TransactionType.Withdrawal,
                $"Saque realizado",
                null
                ));
        }

        private void IncreaseBalance(decimal amount)
        {
            Balance += amount;
        }

        private void DecreaseBalance(decimal amount)
        {
            Balance -= amount;
        }

        private void AddTransaction(BankAccountTransaction transaction)
        {
            Transactions.Add(transaction);
        }
    }
}
