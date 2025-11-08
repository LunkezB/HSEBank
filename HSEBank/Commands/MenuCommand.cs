using HSEBank.Exceptions;
using HSEBank.UI;

namespace HSEBank.Commands
{
    public sealed class MenuCommand(IEnumerable<MenuItem> items, IInput input, CommandManager manager)
        : ICommand
    {
        public string Name => "Главное меню";

        public void Execute()
        {
            Dictionary<string, MenuItem> dict = items.ToDictionary(i => i.Key, i => i);

            while (true)
            {
                PrintMenu(items);
                string choice = input.Ask("Выбор: ").Trim();

                if (string.Equals(choice, "q", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                try
                {
                    if (dict.TryGetValue(choice, out MenuItem? item))
                    {
                        ICommand core = item.BuildCommand();
                        if (core is IUndoableCommand u)
                        {
                            manager.Execute(new TimingUndoableDecorator(u));
                        }
                        else
                        {
                            manager.Execute(new TimingDecorator(core));
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неизвестная команда.");
                    }
                }
                catch (DomainException ex)
                {
                    Console.WriteLine($"Ошибка домена: {ex.Message}");
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Ошибка формата: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
                }
            }
        }

        private static void PrintMenu(IEnumerable<MenuItem> items)
        {
            Console.WriteLine("\n=== Меню ===");
            foreach (MenuItem m in items)
            {
                Console.WriteLine($"{m.Key}) {m.Title}");
            }

            Console.WriteLine("q) Выход");
        }
    }
}