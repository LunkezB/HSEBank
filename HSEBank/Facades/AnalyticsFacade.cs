using HSEBank.Models;
using HSEBank.Repositories;
using System.Globalization;

namespace HSEBank.Facades
{
    public sealed class AnalyticsFacade(
        IOperationRepository operations,
        ICategoryRepository categories) : IAnalyticsFacade
    {
        public decimal NetIncome(DateTime from, DateTime to)
        {
            return operations.GetByPeriod(from, to).Sum(o => o.SignedAmount);
        }

        public IEnumerable<(Category Category, decimal Total)> GroupByCategory(DateTime from, DateTime to)
        {
            return operations.GetByPeriod(from, to)
                .GroupBy(o => o.CategoryId)
                .Select(g => (categories.Get(g.Key), g.Sum(o => o.SignedAmount)))
                .OrderByDescending(t => Math.Abs(t.Item2));
        }

        public IEnumerable<(Category Category, decimal Total)> TopCategories(DateTime from, DateTime to, int topN = 5)
        {
            return GroupByCategory(from, to).Take(topN);
        }


        public IEnumerable<(string Period, decimal Total)> MonthlyTrend(DateTime fromInclusive, DateTime toInclusive)
        {
            IEnumerable<(string Period, decimal Total)> groups = operations.GetByPeriod(fromInclusive, toInclusive)
                .GroupBy(o => new { o.Date.Year, o.Date.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => (
                    Period: new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MM-yyyy", CultureInfo.InvariantCulture),
                    Total: g.Sum(o => o.SignedAmount)));

            return groups;
        }
    }
}