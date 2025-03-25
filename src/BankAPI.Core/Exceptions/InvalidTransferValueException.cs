namespace BankAPI.Core.Exceptions
{
    public class InvalidTransferValueException : Exception
    {
        public InvalidTransferValueException(string? message) : base(message)
        {
        }
    }
}
