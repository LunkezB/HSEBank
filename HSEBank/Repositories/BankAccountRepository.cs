using HSEBank.Exceptions;
using HSEBank.Models;

namespace HSEBank.Repositories
{
    public sealed class BankAccountRepository : IBankAccountRepository
    {
        private readonly Dictionary<Guid, BankAccount> _data = new();

        public BankAccount Add(BankAccount e)
        {
            if (NameExists(e.Name))
            {
                throw new DuplicateException($"Счёт с именем «{e.Name}» уже существует.");
            }

            _data[e.Id] = e;
            return e;
        }

        public void Update(BankAccount e)
        {
            if (!_data.ContainsKey(e.Id))
            {
                throw new NotFoundException("Счёт не найден.");
            }

            if (NameExists(e.Name, e.Id))
            {
                throw new DuplicateException($"Счёт с именем «{e.Name}» уже существует.");
            }

            _data[e.Id] = e;
        }

        public void Remove(Guid id)
        {
            if (!_data.Remove(id))
            {
                throw new NotFoundException("Счёт не найден.");
            }
        }

        public BankAccount Get(Guid id)
        {
            return !_data.TryGetValue(id, out BankAccount? e) ? throw new NotFoundException("Счёт не найден.") : e;
        }

        public IReadOnlyCollection<BankAccount> GetAll()
        {
            return _data.Values.OrderBy(v => v.Name).ToArray();
        }

        public bool Exists(Guid id)
        {
            return _data.ContainsKey(id);
        }

        public bool NameExists(string name, Guid? exceptId = null)
        {
            return _data.Values.Any(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && a.Id != exceptId);
        }
    }
}