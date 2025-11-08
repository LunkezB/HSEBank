using HSEBank.Facades;

namespace HSEBank.Commands.AnalyticsCommand
{
    public sealed class MonthlyTrendCommand(IAnalyticsFacade analytics, DateTime from, DateTime to) : ICommand
    {
        public string Name => "Аналитика: динамика по месяцам";

        public void Execute()
        {
            List<(string Period, decimal Total)> rows = analytics.MonthlyTrend(from, to).ToList();

            Console.WriteLine($"\nДинамика с {from:dd-MM-yyyy} по {to:dd-MM-yyyy}:");
            if (rows.Count == 0)
            {
                Console.WriteLine("Нет данных.");
                return;
            }

            Console.WriteLine("Месяц   | Итого");
            Console.WriteLine("--------+-----------------");
            foreach ((string period, decimal total) in rows)
            {
                Console.WriteLine($"{period,-6} | {total,15}");
            }
        }
    }
}