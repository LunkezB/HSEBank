using HSEBank.Facades;
using HSEBank.Models;

namespace HSEBank.Commands.AnalyticsCommand
{
    public sealed class GroupByCategoryCommand(IAnalyticsFacade analytics, DateTime from, DateTime to) : ICommand
    {
        public string Name => "Аналитика: группировка по категориям";

        public void Execute()
        {
            List<(Category Category, decimal Total)> rows = analytics.GroupByCategory(from, to).ToList();

            Console.WriteLine($"\nГруппировка по категориям за период {from:dd-MM-yyyy} .. {to:dd-MM-yyyy}:");
            if (rows.Count == 0)
            {
                Console.WriteLine("Нет данных.");
                return;
            }

            Console.WriteLine("Категория                               | Итого");
            Console.WriteLine("----------------------------------------+-----------------");
            foreach ((Category cat, decimal total) in rows)
            {
                Console.WriteLine($"{cat.Name,-40} | {total,15}");
            }
        }
    }
}