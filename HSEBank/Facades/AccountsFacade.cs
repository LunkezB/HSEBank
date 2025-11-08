using HSEBank.Exceptions;
using HSEBank.Factories;
using HSEBank.Models;
using HSEBank.Repositories;

namespace HSEBank.Facades
{
    public sealed class AccountsFacade(
        IBankAccountRepository accounts,
        IOperationRepository operations,
        IEntityFactory factory) : IAccountsFacade
    {
        public BankAccount Create(string name, decimal initialBalance = 0m)
        {
            BankAccount acc = factory.CreateAccount(name, initialBalance);
            accounts.Add(acc);
            return acc;
        }

        public void Rename(Guid accountId, string newName)
        {
            BankAccount acc = accounts.Get(accountId);
            acc.Rename(newName);
            accounts.Update(acc);
        }

        public void Delete(Guid accountId, bool cascadeOperations = true)
        {
            if (!accounts.Exists(accountId))
            {
                throw new NotFoundException("Счёт не найден.");
            }

            if (cascadeOperations)
            {
                foreach (Operation op in operations.GetByAccount(accountId).ToList())
                {
                    operations.Remove(op.Id);
                }
            }
            else
            {
                if (operations.GetByAccount(accountId).Count != 0)
                {
                    throw new ValidationException("Нельзя удалить счёт: есть связанные операции.");
                }
            }

            accounts.Remove(accountId);
        }

        public IReadOnlyCollection<BankAccount> GetAll()
        {
            return accounts.GetAll();
        }
    }
}