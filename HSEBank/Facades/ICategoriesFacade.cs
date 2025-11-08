using HSEBank.Models;

namespace HSEBank.Facades
{
    public interface ICategoriesFacade
    {
        Category Create(CategoryType type, string name);
        void Delete(Guid categoryId);
        IReadOnlyCollection<Category> GetAll();
    }
}