using BankAPI.Core.Entities;

namespace BankAPI.Application.Dtos.ViewModels
{
    public class BankAccountViewModel
    {
        public BankAccountViewModel(int id, string name, string document, decimal balance, DateTime openingDate, string status)
        {
            Id = id;
            Name = name;
            Document = document;
            Balance = balance;
            OpeningDate = openingDate;
            Status = status;
        }

        public int Id { get; private set; }

        public string Name { get; private set; }

        public string Document { get; private set; }

        public decimal Balance { get; private set; }

        public DateTime OpeningDate { get; private set; }

        public string Status { get; private set; }

        public static BankAccountViewModel FromEntity(BankAccount bankAccount)
        {
            return new BankAccountViewModel(
                bankAccount.Id, 
                bankAccount.Name, 
                bankAccount.Document, 
                bankAccount.Balance, 
                bankAccount.OpeningDate, 
                bankAccount.Status.ToString());
        }
    }
}
