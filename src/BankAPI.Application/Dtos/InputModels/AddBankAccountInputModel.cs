using BankAPI.Core.Entities;
using System.Text.Json.Serialization;

namespace BankAPI.Application.Dtos.InputModels
{
    public class AddBankAccountInputModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("document")]
        public string Document { get; set; }

        public BankAccount ToEntity()
        {
            return new BankAccount(Name, Document);
        }
    }
}
