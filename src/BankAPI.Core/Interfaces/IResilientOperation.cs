namespace BankAPI.Core.Interfaces
{
    public interface IResilientOperation
    {
        Task ExecuteAsync(Func<Task> operation);
    }
}
