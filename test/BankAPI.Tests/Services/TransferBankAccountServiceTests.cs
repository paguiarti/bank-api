using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Services;
using BankAPI.Core.Entities;
using BankAPI.Core.Enums;
using BankAPI.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankAPI.Tests.Services
{
    public class TransferBankAccountServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<TransferBankAccountService>> _loggerMock;
        private readonly TransferBankAccountService _transferBankAccountService;

        public TransferBankAccountServiceTests()
        {
            _loggerMock = new Mock<ILogger<TransferBankAccountService>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _transferBankAccountService = new TransferBankAccountService(_unitOfWorkMock.Object, _loggerMock.Object);

        }

        [Fact]
        public async Task TransferBankAccountAsync_ShouldReturnFail_WhenDestinationAccountNotFound()
        {
            // Arrange
            var inputModel = new TransferBankAccountInputModel
            {
                BankAccountIdFrom = 1000,
                BankAccountIdTo = 1001,
                Amount = 200
            };

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdFrom))
                .ReturnsAsync(new BankAccount("Paulo", "14526988790"));

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdTo))
                .ReturnsAsync((BankAccount?)null);

            // Act
            var result = await _transferBankAccountService.TransferBankAccountAsync(inputModel);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Conta de destino não encontrada.", result.Message);

            _unitOfWorkMock
                .Verify(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdFrom), Times.Once());

            _unitOfWorkMock
                .Verify(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdTo), Times.Once());
        }

        [Fact]
        public async Task TransferBankAccountAsync_ShouldReturnFail_WhenSourceAccountNotFound()
        {
            // Arrange
            var inputModel = new TransferBankAccountInputModel
            {
                BankAccountIdFrom = 1000,
                BankAccountIdTo = 1001,
                Amount = 200
            };

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdFrom))
                .ReturnsAsync((BankAccount?)null);

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdTo))
                .ReturnsAsync(new BankAccount("Paulo", "14526988790"));

            // Act
            var result = await _transferBankAccountService.TransferBankAccountAsync(inputModel);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Conta de origem não encontrada.", result.Message);

            _unitOfWorkMock
                .Verify(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdFrom), Times.Once());

            _unitOfWorkMock
                .Verify(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdTo), Times.Once());
        }

        [Fact]
        public async Task TransferBankAccountAsync_ShouldCommit_WhenTransferIsValid()
        {
            // Arrange
            var inputModel = new TransferBankAccountInputModel
            {
                BankAccountIdFrom = 1000,
                BankAccountIdTo = 1001,
                Amount = 200
            };

            var bankAccountFrom = CreateActiveAccount("Paulo", "14758299638");            

            //Garante o saldo suficiente
            if (bankAccountFrom.Balance < inputModel.Amount)
            {
                bankAccountFrom.Deposit(inputModel.Amount - bankAccountFrom.Balance);
            }
            
            var bankAccountTo = CreateActiveAccount("Naiara", "47520399864");

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdFrom))
                .ReturnsAsync(bankAccountFrom);

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdTo))
                .ReturnsAsync(bankAccountTo);

            _unitOfWorkMock
                .Setup(u => u.ExecuteResilientlyAsync(It.IsAny<Func<Task>>()))
                .Returns(async (Func<Task> operation) => await operation());

            // Act
            var result = await _transferBankAccountService.TransferBankAccountAsync(inputModel);

            // Assert
            Assert.True(result.Success);
            _unitOfWorkMock.Verify(u => u.CommitTransactionAsync(), Times.Once);
            _unitOfWorkMock.Verify(u => u.RollBackTransactionAsync(), Times.Never);
        }

        [Fact]
        public async Task TransferBankAccountAsync_ShouldReturnFail_WhenInsufficientFunds()
        {
            // Arrange
            var inputModel = new TransferBankAccountInputModel
            {
                BankAccountIdFrom = 1000,
                BankAccountIdTo = 1001,
                Amount = 200
            };

            var bankAccountFrom = CreateActiveAccount("Paulo", "14758299638");
            bankAccountFrom.Withdrawal(bankAccountFrom.Balance);

            var bankAccountTo = CreateActiveAccount("Naiara", "47520399864");

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdFrom))
                .ReturnsAsync(bankAccountFrom);

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetByIdAsync(inputModel.BankAccountIdTo))
                .ReturnsAsync(bankAccountTo);

            _unitOfWorkMock
                .Setup(u => u.ExecuteResilientlyAsync(It.IsAny<Func<Task>>()))
                .Returns(async (Func<Task> operation) => await operation());

            // Act
            var result = await _transferBankAccountService.TransferBankAccountAsync(inputModel);

            Assert.False(result.Success);
            Assert.Equal("Saldo insuficiente para realizar a transferência.", result.Message);

            _unitOfWorkMock.Verify(u => u.RollBackTransactionAsync(), Times.Once);
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
