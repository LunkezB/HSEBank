using HSEBank.Models;

namespace HSEBank.Facades
{
    public interface IAccountsFacade
    {
        BankAccount Create(string name, decimal initialBalance = 0);
        void Rename(Guid accountId, string newName);
        void Delete(Guid accountId, bool cascadeOperations = true);
        IReadOnlyCollection<BankAccount> GetAll();
    }
}