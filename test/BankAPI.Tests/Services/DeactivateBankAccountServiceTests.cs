using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Services;
using BankAPI.Core.Entities;
using BankAPI.Core.Enums;
using BankAPI.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace BankAPI.Tests.Services
{
    public class DeactivateBankAccountServiceTests
    {
        private readonly DeactivateBankAccountService _deactivateBankAccountService;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public DeactivateBankAccountServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _deactivateBankAccountService = new DeactivateBankAccountService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task DeactivateBankAccountAsync_ShouldReturnSuccess_WhenAccountIsDeactivated()
        {
            // Arrange
            var inputModel = new DeactivateBankAccountInputModel
            {
                Document = "14752866390",
                User = "roberto"
            };

            var bankAccount = new BankAccount("Paulo Aguiar Junior", "14752866390");

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetByDocumentAsync(It.IsAny<string>()))
                .ReturnsAsync(bankAccount);

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.Update(It.IsAny<BankAccount>()));

            // Act
            var result = await _deactivateBankAccountService.DeactivateBankAccountAsync(inputModel);

            // Assert
            _unitOfWorkMock
                .Verify(u => u.BankAccountRepository.GetByDocumentAsync(It.IsAny<string>()), Times.Once);

            _unitOfWorkMock
                .Verify(u => u.BankAccountRepository.Update(It.Is<BankAccount>(b => b.Status == BankAccountStatus.Inactive)), Times.Once);

            _unitOfWorkMock
                .Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeactivateBankAccountAsync_ShouldReturnFail_WhenAccountNotFound()
        {
            // Arrange
            var inputModel = new DeactivateBankAccountInputModel
            {
                Document = "14752866390",
                User = "roberto"
            };

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetByDocumentAsync(It.IsAny<string>()))
                .ReturnsAsync((BankAccount?)null);

            // Act
            var result = await _deactivateBankAccountService.DeactivateBankAccountAsync(inputModel);

            // Assert
            Assert.False(result.Success);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
            Assert.Equal(DeactivateBankAccountService.ACCOUNT_NOT_FOUND_MSG, result.Message);

            _unitOfWorkMock
                .Verify(u => u.BankAccountRepository.Update(It.IsAny<BankAccount>()), Times.Never);

            _unitOfWorkMock
                .Verify(u => u.SaveChangesAsync(), Times.Never);
        }
    }
}
