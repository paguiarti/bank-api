using BankAPI.Application.Dtos.InputModels;
using BankAPI.Application.Services;
using BankAPI.Core.Entities;
using BankAPI.Core.Interfaces;
using BankAPI.Core.Interfaces.Repositories;
using Moq;

namespace BankAPI.Tests.Services
{
    public class GetAllBankAccountServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IBankAccountRepository> _bankAccountRepositoryMock;
        private readonly GetAllBankAccountService _getAllBankAccountService;

        public GetAllBankAccountServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _bankAccountRepositoryMock = new Mock<IBankAccountRepository>();

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository)
                .Returns(_bankAccountRepositoryMock.Object);

            _getAllBankAccountService = new GetAllBankAccountService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnFilteredBankAccounts_WhenNameIsProvided()
        {
            // Arrange
            var nameFiltered = "Aguiar";
            
            var bankAccounts = new List<BankAccount> {
                new("Paulo Aguiar Junior", "39063222890"),
                new("Naiara Barboza", "41175850123"),
                new("Plinio Furlaneto", "21435688798")
            };

            var inputModel = new GetAllBankAccountInputModel
            {
                Name = nameFiltered
            };

            var filteredBankAccounts = bankAccounts
                .Where(b => b.Name.Contains(nameFiltered))
                .ToList();

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetAllAsync(nameFiltered, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(filteredBankAccounts);

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.CountAsync(nameFiltered, It.IsAny<string>()))
                .ReturnsAsync(filteredBankAccounts.Count);

            // Act
            var result = await _getAllBankAccountService.GetAllAsync(inputModel);


            // Asserts
            Assert.NotNull(result.Data);
            Assert.Single(result.Data);
            Assert.Equal("Paulo Aguiar Junior", result.Data.First().Name);
            Assert.Equal("39063222890", result.Data.First().Document);

            _bankAccountRepositoryMock
                .Verify(b => b.GetAllAsync(nameFiltered, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()), Times.Once);

            _bankAccountRepositoryMock
                .Verify(b => b.CountAsync(nameFiltered, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoMatchingBankAccounts()
        {
            // Arrange
            var nameFiltered = "Paulo Aguiar Junior";

            var bankAccounts = new List<BankAccount>();
            var inputModel = new GetAllBankAccountInputModel { Name = nameFiltered };

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.GetAllAsync(nameFiltered, It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(bankAccounts);

            _unitOfWorkMock
                .Setup(u => u.BankAccountRepository.CountAsync(nameFiltered, It.IsAny<string>()))
                .ReturnsAsync(0);

            // Act
            var result = await _getAllBankAccountService.GetAllAsync(inputModel);

            // Asserts
            Assert.NotNull(result.Data);
            Assert.Empty(result.Data);
        }
    }
}
