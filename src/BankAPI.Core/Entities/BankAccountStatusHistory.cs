using BankAPI.Core.Enums;

namespace BankAPI.Core.Entities
{
    public class BankAccountStatusHistory : BaseEntity
    {
        public BankAccountStatusHistory(int bankAccountId, BankAccountStatus status, string user)
        {
            BankAccountId = bankAccountId;
            Status = status;            
            User = user;
            ActionDate = DateTime.Now;
        }

        public int BankAccountId { get; private set; }

        public BankAccountStatus Status { get; private set; }

        public DateTime ActionDate { get; private set; }

        public string User { get; private set; }
    }
}
