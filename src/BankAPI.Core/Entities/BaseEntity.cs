namespace BankAPI.Core.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public int Id { get; private set; }

        public DateTime CreatedAt { get; private set; }
    }
}
