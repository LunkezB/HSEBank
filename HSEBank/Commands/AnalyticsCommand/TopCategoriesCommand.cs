using HSEBank.Facades;
using HSEBank.Models;

namespace HSEBank.Commands.AnalyticsCommand
{
    public sealed class TopCategoriesCommand(IAnalyticsFacade analytics, DateTime from, DateTime to, int topN = 5)
        : ICommand
    {
        public string Name => "Аналитика: топ категорий";

        public void Execute()
        {
            List<(Category Category, decimal Total)> rows = analytics.TopCategories(from, to, topN).ToList();

            Console.WriteLine($"\nТОП-{topN} категорий за период {from:dd-MM-yyyy} .. {to:dd-MM-yyyy}:");
            if (rows.Count == 0)
            {
                Console.WriteLine("Нет данных.");
                return;
            }

            Console.WriteLine("#Категория                            | Итого");
            Console.WriteLine("+-------------------------------------+-----------------");
            int i = 1;
            foreach ((Category cat, decimal total) in rows)
            {
                Console.WriteLine($"{i,2} {cat.Name,-35} | {total,15}");
                i++;
            }
        }
    }
}