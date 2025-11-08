using HSEBank.Models;

namespace HSEBank.Repositories
{
    public interface IOperationRepository : IRepository<Operation>
    {
        IReadOnlyCollection<Operation> GetByAccount(Guid accountId);
        IReadOnlyCollection<Operation> GetByPeriod(DateTime from, DateTime to);
        IReadOnlyCollection<Operation> GetByCategory(Guid categoryId);
    }
}