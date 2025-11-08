using HSEBank.Models;

namespace HSEBank.Facades
{
    public interface IOperationsFacade
    {
        Operation Get(Guid id);
        IReadOnlyCollection<Operation> GetAll();
        IReadOnlyCollection<Operation> GetByPeriod(DateTime from, DateTime to);
        IReadOnlyCollection<Operation> GetByAccount(Guid accountId);
        IReadOnlyCollection<Operation> GetByCategory(Guid categoryId);

        Operation Create(OperationType type, Guid accountId, Guid categoryId, decimal amount, DateTime date,
            string? description);

        void Update(Guid opId, decimal amount, DateTime date, Guid categoryId, string? description);
        void Delete(Guid opId);

        void RecalculateBalance(Guid accountId);
        void RecalculateAllBalances();
    }
}