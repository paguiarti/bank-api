using BankAPI.Core.Entities;
using BankAPI.Core.Enums;
using BankAPI.Core.Exceptions;

namespace BankAPI.Tests.Entities
{
    public class BankAccountTests
    {
        [Fact]
        public void BankAccount_ShouldThrowArgumentException_WhenNameIsEmpty()
        {
            // Arrange
            string nome = "";
            string documento = "12345678901";

            // Act
            var exception = Assert.Throws<ArgumentException>(() => new BankAccount(nome, documento));
            
            // Assert
            Assert.Contains("O nome não pode ser vazio ou nulo.", exception.Message);
        }

        [Fact]
        public void BankAccount_ShouldThrowArgumentException_WhenDocumentIsEmpty()
        {
            // Arrange
            string nome = "Paulo Aguiar Junior";
            string documento = "";

            // Act
            var exception = Assert.Throws<ArgumentException>(() => new BankAccount(nome, documento));
            
            // Assert
            Assert.Contains("O documento não pode ser vazio ou nulo.", exception.Message);
        }

        [Fact]
        public void BankAccount_ShouldHaveInitialBalance_WhenCreated()
        {
            // Arrange
            string nome = "Paulo Aguiar Junior";
            string documento = "12345678901";

            // Act
            var bankAccount = new BankAccount(nome, documento);

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

            // Assert
            var exception = Assert.Throws<InvalidBankAccountStatusException>(() => bankAccount.DeactivateAccount("user"));

            Assert.Contains("Conta bancária não se encontra ativa para ser desativada.", exception.Message);
        }

        [Fact]
        public void BankAccount_ShouldAddStatusHistory_WhenAccountIsDeactivated()
        {
            // Arrange
            var bankAccount = new BankAccount("Paulo Aguiar Junior", "12345678901");

            // Act
            bankAccount.DeactivateAccount("user");

            // Assert
            Assert.Single(bankAccount.StatusHistory);
            Assert.Equal(BankAccountStatus.Inactive, bankAccount.Status);
        }
    }
}
