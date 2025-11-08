using HSEBank.Facades;

namespace HSEBank.Commands.AnalyticsCommand
{
    public sealed class NetIncomeCommand(IAnalyticsFacade analytics, DateTime from, DateTime to) : ICommand
    {
        public string Name => "Аналитика: чистый доход";

        public void Execute()
        {
            decimal value = analytics.NetIncome(from, to);
            Console.WriteLine($"\nЧистый доход за период {from:dd-MM-yyyy} .. {to:dd-MM-yyyy}: {value}");
        }
    }
}