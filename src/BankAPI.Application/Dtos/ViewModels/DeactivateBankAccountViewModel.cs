using System.Text.Json.Serialization;

namespace BankAPI.Application.Dtos.ViewModels
{
    public class DeactivateBankAccountViewModel
    {
        public DeactivateBankAccountViewModel(int id, string document, string status)
        {
            Id = id;
            Document = document;
            Status = status;
        }

        [JsonPropertyName("account_number")]
        public int Id { get; private set; }

        [JsonPropertyName("document")]
        public string Document { get; private set; }

        [JsonPropertyName("status")]
        public string Status { get; private set; }
    }
}
