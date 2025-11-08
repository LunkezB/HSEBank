using HSEBank.Facades;
using HSEBank.Models;
using HSEBank.Repositories;

namespace HSEBank.Commands.CategoryCommand
{
    public sealed class DeleteCategoryCommand(ICategoriesFacade facade, ICategoryRepository repo, Guid id)
        : IUndoableCommand
    {
        private Category? _backup;

        public string Name => "Удалить категорию";

        public void Execute()
        {
            _backup = repo.Get(id);
            facade.Delete(id);
            Console.WriteLine("Категория удалена.");
        }

        public void Undo()
        {
            if (_backup is null)
            {
                return;
            }

            repo.Add(_backup);
            Console.WriteLine("Удаление категории отменено (категория восстановлена).");
        }

        public void Redo()
        {
            Execute();
        }
    }
}