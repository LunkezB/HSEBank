using HSEBank.Exceptions;
using HSEBank.Factories;
using HSEBank.Models;
using HSEBank.Repositories;

namespace HSEBank.Facades
{
    public sealed class OperationsFacade(
        IOperationRepository operations,
        IBankAccountRepository accounts,
        ICategoryRepository categories,
        IEntityFactory factory
    ) : IOperationsFacade
    {
        public Operation Get(Guid id)
        {
            return operations.Get(id);
        }

        public IReadOnlyCollection<Operation> GetAll()
        {
            return operations.GetAll();
        }

        public IReadOnlyCollection<Operation> GetByPeriod(DateTime from, DateTime to)
        {
            return operations.GetByPeriod(from, to);
        }

        public IReadOnlyCollection<Operation> GetByAccount(Guid accountId)
        {
            return operations.GetByAccount(accountId);
        }

        public IReadOnlyCollection<Operation> GetByCategory(Guid categoryId)
        {
            return operations.GetByCategory(categoryId);
        }

        public Operation Create(OperationType type, Guid accountId, Guid categoryId, decimal amount, DateTime date,
            string? description)
        {
            if (!accounts.Exists(accountId))
            {
                throw new NotFoundException("Счёт не найден.");
            }

            if (!categories.Exists(categoryId))
            {
                throw new NotFoundException("Категория не найдена.");
            }

            Category cat = categories.Get(categoryId);
            if ((int)type != (int)cat.Type)
            {
                throw new ValidationException("Тип операции не совпадает с типом категории.");
            }

            Operation op = factory.CreateOperation(type, accountId, categoryId, amount, date, description);
            operations.Add(op);

            BankAccount acc = accounts.Get(accountId);
            acc.ApplyDelta(op.SignedAmount);
            accounts.Update(acc);

            return op;
        }

        public void Update(Guid opId, decimal amount, DateTime date, Guid categoryId, string? description)
        {
            Operation op = operations.Get(opId);
            BankAccount acc = accounts.Get(op.BankAccountId);

            acc.ApplyDelta(-op.SignedAmount);

            if (!categories.Exists(categoryId))
            {
                throw new NotFoundException("Категория не найдена.");
            }

            Category cat = categories.Get(categoryId);
            if ((int)op.Type != (int)cat.Type)
            {
                throw new ValidationException("Тип операции не совпадает с типом категории.");
            }

            op.Update(amount, date, categoryId, description);
            operations.Update(op);

            acc.ApplyDelta(op.SignedAmount);
            accounts.Update(acc);
        }

        public void Delete(Guid opId)
        {
            Operation op = operations.Get(opId);
            BankAccount acc = accounts.Get(op.BankAccountId);
            acc.ApplyDelta(-op.SignedAmount);
            accounts.Update(acc);
            operations.Remove(opId);
        }

        public void RecalculateBalance(Guid accountId)
        {
            BankAccount acc = accounts.Get(accountId);
            decimal sum = operations.GetByAccount(accountId).Sum(o => o.SignedAmount);

            decimal delta = sum - acc.Balance;
            if (delta == 0)
            {
                return;
            }

            acc.ApplyDelta(delta);
            accounts.Update(acc);
        }

        public void RecalculateAllBalances()
        {
            foreach (BankAccount a in accounts.GetAll())
            {
                RecalculateBalance(a.Id);
            }
        }
    }
}