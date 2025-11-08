using HSEBank.Export;
using HSEBank.Facades;
using HSEBank.Models;

namespace HSEBank.Commands.ExportCommand
{
    public sealed class ExportCsvCommand(
        IAccountsFacade accounts,
        ICategoriesFacade categories,
        IOperationsFacade operations,
        string path)
        : ICommand
    {
        public string Name => "Экспорт CSV";

        public void Execute()
        {
            CsvExporter exporter = new();

            foreach (BankAccount a in accounts.GetAll())
            {
                a.Accept(exporter);
            }

            foreach (Category c in categories.GetAll())
            {
                c.Accept(exporter);
            }

            foreach (Operation o in operations.GetAll())
            {
                o.Accept(exporter);
            }

            string text = exporter.GetResult();
            File.WriteAllText(path, text);
            Console.WriteLine($"Экспорт CSV выполнен: {path}");
        }
    }
}