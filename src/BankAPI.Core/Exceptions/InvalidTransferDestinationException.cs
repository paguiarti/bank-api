namespace BankAPI.Core.Exceptions
{
    public class InvalidTransferDestinationException : Exception
    {
        public InvalidTransferDestinationException(string? message) : base(message)
        {
        }
    }
}
