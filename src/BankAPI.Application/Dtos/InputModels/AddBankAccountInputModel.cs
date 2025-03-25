using BankAPI.Core.Entities;
using System.Text.Json.Serialization;

namespace BankAPI.Application.Dtos.InputModels
{
    public class AddBankAccountInputModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("document")]
        public string Document { get; set; } = string.Empty;

        public BankAccount ToEntity()
        {
            return new BankAccount(Name, Document);
        }
    }
}
