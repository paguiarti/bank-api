namespace BankAPI.Core.Exceptions
{
    public class InvalidBankAccountStatusException : Exception
    {
        public InvalidBankAccountStatusException(string message) : base(message)
        {
        }
    }
}
