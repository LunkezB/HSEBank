using HSEBank.Commands;
using HSEBank.Commands.AccountCommand;
using HSEBank.Commands.AnalyticsCommand;
using HSEBank.Commands.CategoryCommand;
using HSEBank.Commands.ExportCommand;
using HSEBank.Commands.ImportCommand;
using HSEBank.Commands.OperationCommand;
using HSEBank.Commands.UtilityCommand;
using HSEBank.Facades;
using HSEBank.Factories;
using HSEBank.Models;
using HSEBank.Repositories;
using HSEBank.UI;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace HSEBank
{
    internal static class Program
    {
        private const string DateFmt = "dd-MM-yyyy";

        private static void Main()
        {
            ServiceCollection services = new();

            services.AddSingleton<IEntityFactory, DomainFactory>();

            services.AddSingleton<IBankAccountRepository, BankAccountRepository>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<IOperationRepository, OperationRepository>();

            services.AddSingleton<IAccountsFacade, AccountsFacade>();
            services.AddSingleton<ICategoriesFacade, CategoriesFacade>();
            services.AddSingleton<IOperationsFacade, OperationsFacade>();
            services.AddSingleton<IAnalyticsFacade, AnalyticsFacade>();

            ServiceProvider sp = services.BuildServiceProvider();

            IAccountsFacade accounts = sp.GetRequiredService<IAccountsFacade>();
            ICategoriesFacade categories = sp.GetRequiredService<ICategoriesFacade>();
            IOperationsFacade operations = sp.GetRequiredService<IOperationsFacade>();
            IAnalyticsFacade analytics = sp.GetRequiredService<IAnalyticsFacade>();
            IBankAccountRepository accRepo = sp.GetRequiredService<IBankAccountRepository>();
            ICategoryRepository catRepo = sp.GetRequiredService<ICategoryRepository>();
            IOperationRepository opRepo = sp.GetRequiredService<IOperationRepository>();

            ConsoleInput input = new();
            CommandManager mgr = new();

            MenuItem[] menu =
            [
                new("1", "Создать счёт", () =>
                {
                    string name = input.Ask("Имя счёта: ");
                    decimal bal = input.AskDecimal("Начальный баланс: ", CultureInfo.InvariantCulture);
                    return new CreateAccountCommand(accounts, name, bal);
                }),
                new("2", "Переименовать счёт", () =>
                {
                    Guid id = input.AskGuid("ID счёта: ");
                    string newName = input.Ask("Новое имя: ");
                    return new RenameAccountCommand(accounts, id, newName);
                }),
                new("3", "Удалить счёт", () =>
                {
                    Guid id = input.AskGuid("ID счёта: ");
                    return new DeleteAccountCommand(accounts, operations, accRepo, opRepo, id);
                }),
                new("4", "Список счетов", () => new ListAccountsCommand(accounts)),

                new("5", "Создать категорию", () =>
                {
                    string name = input.Ask("Имя категории: ");
                    string t = input.Ask("Тип (income/expense): ");
                    CategoryType type = t.Equals("income", StringComparison.OrdinalIgnoreCase)
                        ? CategoryType.Income
                        : CategoryType.Expense;
                    return new CreateCategoryCommand(categories, type, name);
                }),
                new("6", "Удалить категорию", () =>
                {
                    Guid id = input.AskGuid("ID категории: ");
                    return new DeleteCategoryCommand(categories, catRepo, id);
                }),
                new("7", "Список категорий", () => new ListCategoriesCommand(categories)),

                new("8", "Создать операцию", () =>
                {
                    Guid accId = input.AskGuid("ID счёта: ");
                    Guid catId = input.AskGuid("ID категории: ");
                    string t = input.Ask("Тип (income/expense): ");
                    OperationType type = t.Equals("income", StringComparison.OrdinalIgnoreCase)
                        ? OperationType.Income
                        : OperationType.Expense;
                    decimal amount = input.AskDecimal("Сумма: ", CultureInfo.InvariantCulture);
                    DateTime date = input.AskDate($"Дата ({DateFmt}): ", DateFmt, CultureInfo.InvariantCulture);
                    string? desc = input.AskOptional("Описание (опц): ");
                    return new CreateOperationCommand(operations, type, accId, catId, amount, date, desc);
                }),
                new("9", "Удалить операцию", () =>
                {
                    Guid opId = input.AskGuid("ID операции: ");
                    return new DeleteOperationCommand(operations, opId);
                }),

                new("10", "Список операций", () => new ListOperationsCommand(operations, accRepo, catRepo)),

                new("11", "Аналитика: чистый доход за период", () =>
                {
                    DateTime from = input.AskDate($"Дата начала ({DateFmt}): ", DateFmt, CultureInfo.InvariantCulture);
                    DateTime to = input.AskDate($"Дата конца   ({DateFmt}): ", DateFmt, CultureInfo.InvariantCulture);
                    return new NetIncomeCommand(analytics, from, to);
                }),
                new("12", "Аналитика: группировка по категориям", () =>
                {
                    DateTime from = input.AskDate($"Дата начала ({DateFmt}): ", DateFmt, CultureInfo.InvariantCulture);
                    DateTime to = input.AskDate($"Дата конца   ({DateFmt}): ", DateFmt, CultureInfo.InvariantCulture);
                    return new GroupByCategoryCommand(analytics, from, to);
                }),
                new("13", "Аналитика: Топ-N категорий", () =>
                {
                    DateTime from = input.AskDate($"Дата начала ({DateFmt}): ", DateFmt, CultureInfo.InvariantCulture);
                    DateTime to = input.AskDate($"Дата конца   ({DateFmt}): ", DateFmt, CultureInfo.InvariantCulture);
                    int n = input.AskInt("Сколько категорий (N): ");
                    return new TopCategoriesCommand(analytics, from, to, n);
                }),
                new("14", "Аналитика: динамика по месяцам", () =>
                {
                    DateTime from = input.AskDate($"Дата начала ({DateFmt}): ", DateFmt, CultureInfo.InvariantCulture);
                    DateTime to = input.AskDate($"Дата конца  ({DateFmt}): ", DateFmt, CultureInfo.InvariantCulture);
                    return new MonthlyTrendCommand(analytics, from, to);
                }),
                new("15", "Экспорт в CSV", () =>
                {
                    string path = input.Ask("Путь к файлу CSV для сохранения: ");
                    return new ExportCsvCommand(accounts, categories, operations, path);
                }),
                new("16", "Экспорт в JSON", () =>
                {
                    string path = input.Ask("Путь к файлу JSON для сохранения: ");
                    return new ExportJsonCommand(accounts, categories, operations, path);
                }),
                new("17", "Импорт из CSV", () =>
                {
                    string path = input.Ask("Путь к CSV: ");
                    return new ImportCsvCommand(accounts, categories, operations, accRepo, catRepo, path);
                }),
                new("18", "Импорт из JSON", () =>
                {
                    string path = input.Ask("Путь к JSON: ");
                    return new ImportJsonCommand(accounts, categories, operations, accRepo, catRepo, path);
                }),
                new("u", "Undo", () => new UndoCommand(mgr)),
                new("r", "Redo", () => new RedoCommand(mgr)),
                new("p", "Повторить последнюю выполненную команду", () => new RepeatLastCommand(mgr))
            ];
            MenuCommand app = new(menu, input, mgr);
            app.Execute();
        }
    }
}