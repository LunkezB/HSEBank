using HSEBank.Facades;
using HSEBank.Models;

namespace HSEBank.Commands.CategoryCommand
{
    public sealed class CreateCategoryCommand(ICategoriesFacade facade, CategoryType type, string name)
        : IUndoableCommand
    {
        private Guid _createdId;

        public string Name => "Создать категорию";

        public void Execute()
        {
            Category c = facade.Create(type, name);
            _createdId = c.Id;
            Console.WriteLine($"Создана категория: {c.Id} | {name} | {type}");
        }

        public void Undo()
        {
            if (_createdId == Guid.Empty)
            {
                return;
            }

            facade.Delete(_createdId);
            Console.WriteLine("Создание категории отменено (категория удалена).");
        }

        public void Redo()
        {
            Execute();
        }
    }
}