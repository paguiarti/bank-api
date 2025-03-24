using BankAPI.Core.Entities;
using System.Text.Json.Serialization;

namespace BankAPI.Application.Dtos.ViewModels
{
    public class AddBankAccountViewModel
    {
        public AddBankAccountViewModel(int id, string name, string document, decimal balance, DateTime openingDate, string status)
        {
            Id = id;
            Name = name;
            Document = document;
            Balance = balance;
            OpeningDate = openingDate;
            Status = status;
        }

        [JsonPropertyName("account_number")]
        public int Id { get; private set; }

        [JsonPropertyName("name")]
        public string Name { get; private set; }

        [JsonPropertyName("document")]
        public string Document { get; private set; }

        [JsonPropertyName("balance")]
        public decimal Balance { get; private set; }

        [JsonPropertyName("opening_date")]
        public DateTime OpeningDate { get; private set; }

        [JsonPropertyName("status")]
        public string Status { get; private set; }

        public static AddBankAccountViewModel FromEntity(BankAccount bankAccount)
        {
            return new AddBankAccountViewModel(bankAccount.Id, bankAccount.Name, bankAccount.Document, bankAccount.Balance, bankAccount.OpeningDate, bankAccount.Status.ToString());
        }
    }
}
