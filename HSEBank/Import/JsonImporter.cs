using HSEBank.Models;
using System.Globalization;
using System.Text.Json;

namespace HSEBank.Import
{
    public sealed class JsonImporter : DataImporter
    {
        protected override FinanceData Parse(string text)
        {
            JsonSerializerOptions opts = new() { PropertyNameCaseInsensitive = true };

            Root r = JsonSerializer.Deserialize<Root>(text, opts) ?? new Root();

            Dictionary<string, string> accIdToName = new(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, string> catIdToName = new(StringComparer.OrdinalIgnoreCase);

            List<(string Name, decimal InitialBalance)> accs =
                (r.Accounts ?? []).Select(a =>
                {
                    string name = a.Name ?? throw new Exception("account.name обязателен");
                    decimal initial = a.InitialBalance ?? a.Balance ?? 0m;
                    if (!string.IsNullOrWhiteSpace(a.Id))
                    {
                        accIdToName[a.Id!] = name;
                    }

                    return (name, initial);
                }).ToList();

            List<(string Name, CategoryType Type)> cats =
                (r.Categories ?? []).Select(c =>
                {
                    string name = c.Name ?? throw new Exception("category.name обязателен");
                    CategoryType type = ParseCat(c.Type);
                    if (!string.IsNullOrWhiteSpace(c.Id))
                    {
                        catIdToName[c.Id!] = name;
                    }

                    return (name, type);
                }).ToList();

            List<(string AccountName, string CategoryName, OperationType Type, decimal Amount, DateTime Date, string?
                Description)> ops =
                (r.Operations ?? []).Select(o =>
                {
                    OperationType type = ParseOp(o.Type);

                    DateTime date = DateTime.ParseExact(
                        o.Date ?? throw new Exception("operation.date обязателен"),
                        "dd-MM-yyyy", CultureInfo.InvariantCulture);

                    decimal amount = o.Amount;

                    string? desc = string.IsNullOrWhiteSpace(o.Description) ? null : o.Description;

                    string? accName = o.Account;
                    if (string.IsNullOrWhiteSpace(accName) && !string.IsNullOrWhiteSpace(o.BankAccountId))
                    {
                        if (!accIdToName.TryGetValue(o.BankAccountId!, out accName))
                        {
                            throw new Exception($"operation.bankAccountId «{o.BankAccountId}» не найден в accounts.");
                        }
                    }

                    if (string.IsNullOrWhiteSpace(accName))
                    {
                        throw new Exception("operation.account или operation.bankAccountId обязателен");
                    }

                    string? catName = o.Category;
                    if (string.IsNullOrWhiteSpace(catName) && !string.IsNullOrWhiteSpace(o.CategoryId))
                    {
                        if (!catIdToName.TryGetValue(o.CategoryId!, out catName))
                        {
                            throw new Exception($"operation.categoryId «{o.CategoryId}» не найден в categories.");
                        }
                    }

                    if (string.IsNullOrWhiteSpace(catName))
                    {
                        throw new Exception("operation.category или operation.categoryId обязателен");
                    }

                    return (accName!, catName!, type, amount, date, desc);
                }).ToList();

            return new FinanceData(accs, cats, ops);

            static CategoryType ParseCat(string? t)
            {
                return t is null ? CategoryType.Expense :
                    t.Equals("income", StringComparison.OrdinalIgnoreCase) ? CategoryType.Income :
                    t.Equals("expense", StringComparison.OrdinalIgnoreCase) ? CategoryType.Expense :
                    throw new FormatException($"Неизвестный тип категории: {t}. Ожидается income или expense.");
            }

            static OperationType ParseOp(string? t)
            {
                return t is null ? OperationType.Expense :
                    t.Equals("income", StringComparison.OrdinalIgnoreCase) ? OperationType.Income :
                    t.Equals("expense", StringComparison.OrdinalIgnoreCase) ? OperationType.Expense :
                    throw new FormatException($"Неизвестный тип операции: {t}. Ожидается income или expense.");
            }
        }

        private sealed class Root
        {
            public List<JsonAccount>? Accounts { get; init; }
            public List<JsonCategory>? Categories { get; init; }
            public List<JsonOperation>? Operations { get; init; }
        }

        private sealed class JsonAccount
        {
            public string? Id { get; init; }
            public string? Name { get; init; }
            public decimal? InitialBalance { get; init; }
            public decimal? Balance { get; init; }
        }

        private sealed class JsonCategory
        {
            public string? Id { get; init; }
            public string? Name { get; init; }
            public string? Type { get; init; }
        }

        private sealed class JsonOperation
        {
            public string? Id { get; init; }
            public string? Account { get; init; }
            public string? Category { get; init; }
            public string? BankAccountId { get; init; }
            public string? CategoryId { get; init; }

            public string? Type { get; init; }
            public decimal Amount { get; init; }
            public string? Date { get; init; }
            public string? Description { get; init; }
        }
    }
}