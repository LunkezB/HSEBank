using HSEBank.Models;
using System.Globalization;

namespace HSEBank.Import
{
    public sealed class CsvImporter : DataImporter
    {
        protected override FinanceData Parse(string text)
        {
            List<(string Name, decimal InitialBalance)> accounts = [];
            List<(string Name, CategoryType Type)> categories = [];
            List<(string AccountName, string CategoryName, OperationType Type, decimal Amount, DateTime Date, string?
                Description)> operations =
                [];

            Dictionary<string, string> accIdToName = new(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, string> catIdToName = new(StringComparer.OrdinalIgnoreCase);

            string section = "";
            foreach (string raw in text.Split('\n',
                         StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (raw.StartsWith('['))
                {
                    section = raw.Trim();
                    continue;
                }

                if (raw.StartsWith("Id", StringComparison.OrdinalIgnoreCase) ||
                    raw.StartsWith("Name", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string[] cols = raw.Split(',', StringSplitOptions.TrimEntries);

                switch (section)
                {
                    case "[accounts]":
                        {
                            if (cols.Length < 3)
                            {
                                throw new FormatException("Раздел [accounts]: ожидались столбцы Id,Name,Balance.");
                            }

                            string accId = cols[0];
                            string accName = cols[1];
                            decimal bal = decimal.Parse(cols[2], CultureInfo.InvariantCulture);

                            accIdToName[accId] = accName;
                            accounts.Add((accName, bal));
                            break;
                        }

                    case "[categories]":
                        {
                            if (cols.Length < 3)
                            {
                                throw new FormatException("Раздел [categories]: ожидались столбцы Id,Name,Type.");
                            }

                            string catId = cols[0];
                            string catName = cols[1];
                            CategoryType type = cols[2].Equals("income", StringComparison.OrdinalIgnoreCase)
                                ? CategoryType.Income
                                : cols[2].Equals("expense", StringComparison.OrdinalIgnoreCase)
                                    ? CategoryType.Expense
                                    : throw new FormatException(
                                        $"Неизвестный тип операции: {cols[2]}. Ожидается income или expense.");

                            catIdToName[catId] = catName;
                            categories.Add((catName, type));
                            break;
                        }

                    case "[operations]":
                        {
                            if (cols.Length < 7)
                            {
                                throw new FormatException(
                                    "Раздел [operations]: ожидались столбцы Id,Type,BankAccountId,Amount,Date,Description,CategoryId.");
                            }

                            string typeStr = cols[1];
                            OperationType type = typeStr.Equals("income", StringComparison.OrdinalIgnoreCase)
                                ? OperationType.Income
                                : typeStr.Equals("expense", StringComparison.OrdinalIgnoreCase)
                                    ? OperationType.Expense
                                    : throw new FormatException(
                                        $"Неизвестный тип операции: {typeStr}. Ожидается income или expense.");

                            string bankAccountId = cols[2];
                            decimal amount = decimal.Parse(cols[3], CultureInfo.InvariantCulture);
                            DateTime date = DateTime.ParseExact(cols[4], "dd-MM-yyyy", CultureInfo.InvariantCulture);
                            string? desc = string.IsNullOrWhiteSpace(cols[5]) ? null : cols[5];
                            string categoryId = cols[6];

                            if (!accIdToName.TryGetValue(bankAccountId, out string? accName))
                            {
                                throw new FormatException(
                                    $"[operations]: неизвестный BankAccountId {bankAccountId} (нет в [accounts]).");
                            }

                            if (!catIdToName.TryGetValue(categoryId, out string? catName))
                            {
                                throw new FormatException(
                                    $"[operations]: неизвестный CategoryId {categoryId} (нет в [categories]).");
                            }

                            operations.Add((accName, catName, type, amount, date, desc));
                            break;
                        }
                }
            }

            return new FinanceData(accounts, categories, operations);
        }
    }
}