using HSEBank.Exceptions;
using HSEBank.Models;

namespace HSEBank.Repositories
{
    public sealed class CategoryRepository : ICategoryRepository
    {
        private readonly Dictionary<Guid, Category> _data = new();

        public Category Add(Category e)
        {
            if (NameExists(e.Name))
            {
                throw new DuplicateException($"Категория «{e.Name}» уже существует.");
            }

            _data[e.Id] = e;
            return e;
        }

        public void Update(Category e)
        {
            if (!_data.ContainsKey(e.Id))
            {
                throw new NotFoundException("Категория не найдена.");
            }

            if (NameExists(e.Name, e.Id))
            {
                throw new DuplicateException($"Категория «{e.Name}» уже существует.");
            }

            _data[e.Id] = e;
        }

        public void Remove(Guid id)
        {
            if (!_data.Remove(id))
            {
                throw new NotFoundException("Категория не найдена.");
            }
        }

        public Category Get(Guid id)
        {
            return !_data.TryGetValue(id, out Category? e) ? throw new NotFoundException("Категория не найдена.") : e;
        }

        public IReadOnlyCollection<Category> GetAll()
        {
            return _data.Values.OrderBy(v => v.Name).ToArray();
        }

        public bool Exists(Guid id)
        {
            return _data.ContainsKey(id);
        }

        public bool NameExists(string name, Guid? exceptId = null)
        {
            return _data.Values.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && c.Id != exceptId);
        }

        public Category GetByName(string name)
        {
            Category? c = _data.Values.FirstOrDefault(v => v.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return c ?? throw new NotFoundException($"Категория «{name}» не найдена.");
        }
    }
}