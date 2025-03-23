using BankAPI.Core.Entities;

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
            Assert.Equal(BankAccount.BONIFICACAO_INICIAL, bankAccount.Balance);
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
    }
}
