using HSEBank.Facades;
using HSEBank.Models;

namespace HSEBank.Commands.CategoryCommand
{
    public sealed class ListCategoriesCommand(ICategoriesFacade categories) : ICommand
    {
        public string Name => "Список категорий";

        public void Execute()
        {
            List<Category> all = categories.GetAll().ToList();
            if (all.Count == 0)
            {
                Console.WriteLine("Категорий нет.");
                return;
            }

            Console.WriteLine("ID                                   | Имя                       | Тип");
            Console.WriteLine("-------------------------------------+---------------------------+-----------");
            foreach (Category c in all)
            {
                Console.WriteLine($"{c.Id} | {c.Name,-25} | {c.Type}");
            }
        }
    }
}