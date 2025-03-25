using System.Text.Json.Serialization;

namespace BankAPI.Application.Dtos.InputModels
{
    public class DeactivateBankAccountInputModel
    {
        [JsonPropertyName("document")]
        public string Document { get; set; } = string.Empty;

        [JsonPropertyName("user")]
        public string User { get; set; } = string.Empty;
    }
}
