using HSEBank.Facades;
using HSEBank.Import;
using HSEBank.Repositories;

namespace HSEBank.Commands.ImportCommand
{
    public sealed class ImportCsvCommand(
        IAccountsFacade accounts,
        ICategoriesFacade categories,
        IOperationsFacade operations,
        IBankAccountRepository accRepo,
        ICategoryRepository catRepo,
        string path)
        : ICommand
    {
        public string Name => "Импорт CSV";

        public void Execute()
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Файл не найден.", path);
            }

            string text = File.ReadAllText(path);
            CsvImporter importer = new();
            importer.Import(text, accounts, categories, operations, accRepo, catRepo);

            Console.WriteLine($"Импорт CSV выполнен из: {path}");
        }
    }
}