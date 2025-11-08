using HSEBank.Exceptions;
using HSEBank.Models;

namespace HSEBank.Repositories
{
    public sealed class OperationRepository : IOperationRepository
    {
        private readonly Dictionary<Guid, Operation> _data = new();

        public Operation Add(Operation e)
        {
            _data[e.Id] = e;
            return e;
        }

        public void Update(Operation e)
        {
            if (!_data.ContainsKey(e.Id))
            {
                throw new NotFoundException("Операция не найдена.");
            }

            _data[e.Id] = e;
        }

        public void Remove(Guid id)
        {
            if (!_data.Remove(id))
            {
                throw new NotFoundException("Операция не найдена.");
            }
        }

        public Operation Get(Guid id)
        {
            if (!_data.TryGetValue(id, out Operation? e))
            {
                throw new NotFoundException("Операция не найдена.");
            }

            return e;
        }

        public IReadOnlyCollection<Operation> GetAll()
        {
            return _data.Values.OrderBy(v => v.Date).ToArray();
        }

        public bool Exists(Guid id)
        {
            return _data.ContainsKey(id);
        }

        public IReadOnlyCollection<Operation> GetByAccount(Guid accountId)
        {
            return _data.Values.Where(o => o.BankAccountId == accountId).OrderBy(o => o.Date).ToArray();
        }

        public IReadOnlyCollection<Operation> GetByPeriod(DateTime from, DateTime to)
        {
            return _data.Values.Where(o => o.Date >= from && o.Date <= to).OrderBy(o => o.Date).ToArray();
        }

        public IReadOnlyCollection<Operation> GetByCategory(Guid categoryId)
        {
            return _data.Values.Where(o => o.CategoryId == categoryId).OrderBy(o => o.Date).ToArray();
        }
    }
}