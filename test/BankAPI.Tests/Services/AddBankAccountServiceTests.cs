using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Services;
using BankAPI.Core.Entities;
using BankAPI.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace BankAPI.Tests.Services
{
    public class AddBankAccountServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AddBankAccountService _addBankAccountService;
        
        public AddBankAccountServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _addBankAccountService = new AddBankAccountService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnFailResponse_WhenAccountExists()
        {
            // Arrange
            var inputModel = new AddBankAccountInputModel
            {
                Name = "Willian Baptista",
                Document = "36412544780"
            };

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.ExistsByDocumentAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _addBankAccountService.AddAsync(inputModel);

            
            // Assert
            Assert.Equal(StatusCodes.Status409Conflict, result.StatusCode);
            Assert.Equal(AddBankAccountService.ACCOUNT_ALREADY_EXISTS_MSG, result.Message);

            _unitOfWorkMock.Verify(u => u.BankAccountRepository.AddAsync(It.IsAny<BankAccount>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSuccessResponse_WhenAccountIsCreated()
        {
            // Arrange
            var inputModel = new AddBankAccountInputModel
            {
                Name = "Paulo Aguiar Junior",
                Document = "39063222890"
            };

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.ExistsByDocumentAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.AddAsync(It.IsAny<BankAccount>()))
                .ReturnsAsync(new BankAccount(inputModel.Name, inputModel.Document));

            _unitOfWorkMock
                .Setup(u => u.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _addBankAccountService.AddAsync(inputModel);

            // Assert
            Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
            Assert.Equal(AddBankAccountService.ACCOUNT_CREATED_MSG, result.Message);

            _unitOfWorkMock
                .Verify(u => u.BankAccountRepository.AddAsync(It.Is<BankAccount>(b => b.Document == inputModel.Document && b.Name == inputModel.Name)), Times.Once);

            _unitOfWorkMock
                .Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
