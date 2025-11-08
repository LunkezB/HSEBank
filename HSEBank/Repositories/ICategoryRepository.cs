using HSEBank.Models;

namespace HSEBank.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        bool NameExists(string name, Guid? exceptId = null);
        Category GetByName(string name);
    }
}