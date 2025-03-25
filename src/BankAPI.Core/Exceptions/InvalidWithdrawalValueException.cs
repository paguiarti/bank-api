namespace BankAPI.Core.Exceptions
{
    public class InvalidWithdrawalValueException : Exception
    {
        public InvalidWithdrawalValueException(string? message) : base(message)
        {
        }
    }
}
