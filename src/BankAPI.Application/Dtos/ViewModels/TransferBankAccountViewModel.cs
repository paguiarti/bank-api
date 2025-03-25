using System.Text.Json.Serialization;

namespace BankAPI.Application.Dtos.ViewModels
{
    public class TransferBankAccountViewModel
    {
        public TransferBankAccountViewModel(int bankAccountIdFrom, int bankAccountIdTo, decimal amount)
        {
            BankAccountIdFrom = bankAccountIdFrom;
            BankAccountIdTo = bankAccountIdTo;
            Amount = amount;
        }

        [JsonPropertyName("account_number_from")]
        public int BankAccountIdFrom { get; private set; }

        [JsonPropertyName("account_number_to")]
        public int BankAccountIdTo { get; private set; }

        [JsonPropertyName("amount_transfered")]
        public decimal Amount { get; private set; }
    }
}
