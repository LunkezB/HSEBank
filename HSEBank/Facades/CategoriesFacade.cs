using HSEBank.Exceptions;
using HSEBank.Factories;
using HSEBank.Models;
using HSEBank.Repositories;

namespace HSEBank.Facades
{
    public sealed class CategoriesFacade(
        ICategoryRepository categories,
        IOperationRepository operations,
        IEntityFactory factory) : ICategoriesFacade
    {
        public Category Create(CategoryType type, string name)
        {
            Category c = factory.CreateCategory(type, name);
            categories.Add(c);
            return c;
        }

        public void Delete(Guid categoryId)
        {
            if (operations.GetByCategory(categoryId).Count != 0)
            {
                throw new ValidationException("Нельзя удалить категорию: есть связанные операции.");
            }

            categories.Remove(categoryId);
        }

        public IReadOnlyCollection<Category> GetAll()
        {
            return categories.GetAll();
        }

        public void Rename(Guid categoryId, string newName)
        {
            Category cat = categories.Get(categoryId);
            cat.Rename(newName);
            categories.Update(cat);
        }
    }
}