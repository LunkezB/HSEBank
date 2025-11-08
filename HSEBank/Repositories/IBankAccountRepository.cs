using HSEBank.Models;

namespace HSEBank.Repositories
{
    public interface IBankAccountRepository : IRepository<BankAccount>
    {
        bool NameExists(string name, Guid? exceptId = null);
    }
}