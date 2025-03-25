using BankAPI.Core.Entities;
using BankAPI.Core.Enums;
using BankAPI.Core.Exceptions;

namespace BankAPI.Tests.Entities
{
    public class BankAccountTests
    {
        [Fact]
        public void BankAccount_ShouldBeActive_WhenCreated()
        {
            var account = new BankAccount("Paulo Aguiar Junior", "39063222890");
            Assert.Equal(BankAccountStatus.Active, account.Status);
        }


        [Theory]
        [InlineData("", "12345678901", "O nome não pode ser vazio ou nulo.")]
        [InlineData("Paulo", "", "O documento não pode ser vazio ou nulo.")]
        [InlineData(null, "12345678901", "O nome não pode ser vazio ou nulo.")]
        public void BankAccount_ShouldThrowArgumentException_WhenNameOrDocumentAreEmpty(string name, string document, string expectedError)
        {
            var exception = Assert.Throws<ArgumentException>(() => new BankAccount(name, document));
            Assert.Contains(expectedError, exception.Message);
        }

        [Fact]
        public void BankAccount_ShouldHaveInitialBalance_WhenCreated()
        {
            // Arrange
            string name = "Paulo Aguiar Junior";
            string document = "12345678901";

            // Act
            var bankAccount = new BankAccount(name, document);

            // Assert
            Assert.Equal(BankAccount.INITIAL_BONUS, bankAccount.Balance);
        }

        [Fact]
        public void BankAccount_ShouldHaveOpeningDate_WhenCreated()
        {
            // Arrange
            string name = "João Silva";
            string document = "12345678901";

            // Act
            var bankAccount = new BankAccount(name, document);

            // Assert
            Assert.InRange(bankAccount.OpeningDate, DateTime.Now.AddSeconds(-5), DateTime.Now);
        }

        [Fact]
        public void BankAccount_ShouldThrowInvalidBankAccountStatusException_WhenDeactivateCalledAndStatusIsNotActive()
        {
            // Arrange
            var bankAccount = new BankAccount("Paulo Aguiar Junior", "12345678901");

            // Act
            bankAccount.DeactivateAccount("user");

            // Act & Assert
            // Tenta desativar novamente
            var exception = Assert.Throws<InvalidBankAccountStatusException>(() => bankAccount.DeactivateAccount("user"));

            // Assert
            Assert.Contains("Conta bancária não se encontra ativa para ser desativada.", exception.Message);
        }

        [Fact]
        public void Deactivate_ShouldAddStatusHistory_WhenAccountIsDeactivated()
        {
            // Arrange
            var bankAccount = new BankAccount("Paulo Aguiar Junior", "12345678901");

            // Act
            bankAccount.DeactivateAccount("user");

            // Assert
            Assert.Single(bankAccount.StatusHistory);
            Assert.Equal(BankAccountStatus.Inactive, bankAccount.Status);
        }

        [Theory]
        [InlineData(500)]
        [InlineData(1200)]
        [InlineData(50)]
        public void TransferTo_ShouldUpdateBalances_WhenAccountsAreActiveAndHaveSufficientFunds(decimal valueToTransfer)
        {
            // Arrange
            var bankAccountFrom = CreateActiveAccount("José", "12436584591");
            if (bankAccountFrom.Balance < valueToTransfer)
            {
                bankAccountFrom.Deposit(valueToTransfer - bankAccountFrom.Balance);
            }

            var bankAccountTo = CreateActiveAccount("Elias", "57486922540");

            var bankAccountFromInitialBalance = bankAccountFrom.Balance;
            var bankAccountToInitialBalance = bankAccountTo.Balance;

            // Act
            bankAccountFrom.TransferTo(bankAccountTo, valueToTransfer);

            // Assert
            Assert.Equal(bankAccountFrom.Balance, bankAccountFromInitialBalance - valueToTransfer);
            Assert.Equal(bankAccountTo.Balance, bankAccountToInitialBalance + valueToTransfer);

            var fromTransaction = bankAccountFrom.Transactions.Last();
            var toTransaction = bankAccountTo.Transactions.Last();

            Assert.Equal(TransactionType.TransferOut, fromTransaction.TransactionType);
            Assert.Equal(TransactionType.TransferIn, toTransaction.TransactionType);
            Assert.Equal(valueToTransfer, fromTransaction.Amount);
            Assert.Equal(valueToTransfer, toTransaction.Amount);
            Assert.Equal(fromTransaction.TransactionPairId, toTransaction.TransactionPairId);
            Assert.Equal($"Transferência realizada para a conta: {bankAccountTo.Id}", fromTransaction.Description);
            Assert.Equal($"Transferência recebida da conta: {bankAccountFrom.Id}", toTransaction.Description);
        }

        [Fact]
        public void TransferTo_ShouldThrowInsufficientFundsException_WhenInsufficientFunds()
        {
            // Arrange
            var valueToTransfer = 200;
            var bankAccountFrom = CreateActiveAccount("José", "12436584591");
            if (bankAccountFrom.Balance > 0)
            {
                bankAccountFrom.Withdrawal(bankAccountFrom.Balance);
            }

            var bankAccountTo = CreateActiveAccount("Elias", "57486922540");

            // Act & Assert
            var exception = Assert.Throws<InsufficientFundsException>(() =>
                bankAccountFrom.TransferTo(bankAccountTo, valueToTransfer));

            Assert.Equal("Saldo insuficiente para realizar a transferência.", exception.Message);
        }

        [Fact]
        public void TransferTo_ShouldThrowInvalidBankAccountStatusException_WhenDestinationIsNotActive()
        {
            // Arrange
            var valueToTransfer = 10;

            var bankAccountFrom = CreateActiveAccount("José", "12436584591");
            if (bankAccountFrom.Balance < valueToTransfer)
            {
                bankAccountFrom.Deposit(valueToTransfer - bankAccountFrom.Balance);
            }

            var bankAccountTo = new BankAccount("Paulo", "47598633250");
            if (bankAccountTo.Status != BankAccountStatus.Inactive)
            {
                bankAccountTo.DeactivateAccount("user");
            }

            // Act & Assert
            var exception = Assert.Throws<InvalidBankAccountStatusException>(() =>
                bankAccountFrom.TransferTo(bankAccountTo, valueToTransfer));

            // Assert            
            Assert.Equal("Conta destino deve estar ativa para receber uma transferência.", exception.Message);
        }

        [Fact]
        public void TransferTo_ShouldThrowInvalidBankAccountStatusException_WhenSourceAccountIsNotActive()
        {
            // Arrange
            var valueToTransfer = 10;

            var bankAccountFrom = CreateActiveAccount("José", "12436584591");
            if (bankAccountFrom.Balance < valueToTransfer)
            {
                bankAccountFrom.Deposit(valueToTransfer - bankAccountFrom.Balance);
            }

            if (bankAccountFrom.Status != BankAccountStatus.Inactive)
            {
                bankAccountFrom.DeactivateAccount("user");
            }

            var bankAccountTo = CreateActiveAccount("Elias", "57486922540");

            // Act & Assert
            var exception = Assert.Throws<InvalidBankAccountStatusException>(() =>
                bankAccountFrom.TransferTo(bankAccountTo, valueToTransfer));

            // Assert            
            Assert.Equal("Conta origem deve estar ativa para realizar uma transferência.", exception.Message);
        }

        private BankAccount CreateActiveAccount(string name, string document)
        {
            var bankAccount = new BankAccount(name, document);
            if (bankAccount.Status != BankAccountStatus.Active)
            {
                bankAccount.ActivateAccount("user");
            }

            return bankAccount;
        }
    }
}
