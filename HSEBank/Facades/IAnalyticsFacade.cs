using HSEBank.Models;

namespace HSEBank.Facades
{
    public interface IAnalyticsFacade
    {
        decimal NetIncome(DateTime from, DateTime to);

        IEnumerable<(Category Category, decimal Total)> GroupByCategory(DateTime from, DateTime to);

        IEnumerable<(Category Category, decimal Total)> TopCategories(DateTime from, DateTime to, int topN = 5);

        IEnumerable<(string Period, decimal Total)> MonthlyTrend(DateTime fromInclusive, DateTime toInclusive);
    }
}