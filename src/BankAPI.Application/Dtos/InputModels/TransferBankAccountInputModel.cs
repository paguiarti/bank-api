using System.Text.Json.Serialization;

namespace BankAPI.Application.Dtos.InputModels
{
    public class TransferBankAccountInputModel
    {
        [JsonPropertyName("account_number_from")]
        public int BankAccountIdFrom { get; set; }

        [JsonPropertyName("account_number_to")]
        public int BankAccountIdTo { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }
    }
}
