using System.Text.Json.Serialization;

namespace BankAPI.Application.Dtos.InputModels
{
    public class DeactivateBankAccountInputModel
    {
        [JsonPropertyName("account_number")]
        public int BankAccountId { get; set; }


        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;
    }
}
