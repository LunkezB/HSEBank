using HSEBank.Models;

namespace HSEBank.Import
{
    public record FinanceData(
        List<(string Name, decimal InitialBalance)> Accounts,
        List<(string Name, CategoryType Type)> Categories,
        List<(string AccountName, string CategoryName, OperationType Type, decimal Amount, DateTime Date, string?
            Description)> Operations
    );
}