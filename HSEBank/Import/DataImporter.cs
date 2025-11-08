using HSEBank.Facades;
using HSEBank.Models;
using HSEBank.Repositories;

namespace HSEBank.Import
{
    public abstract class DataImporter
    {
        public void Import(
            string text,
            IAccountsFacade accountsFacade,
            ICategoriesFacade categoriesFacade,
            IOperationsFacade operationsFacade,
            IBankAccountRepository accRepo,
            ICategoryRepository catRepo)
        {
            FinanceData parsedText = Parse(text);

            foreach ((string Name, decimal InitialBalance) a in parsedText.Accounts.Where(a =>
                         !accRepo.NameExists(a.Name)))
            {
                accountsFacade.Create(a.Name, a.InitialBalance);
            }

            foreach ((string Name, CategoryType Type) c in
                     parsedText.Categories.Where(c => !catRepo.NameExists(c.Name)))
            {
                categoriesFacade.Create(c.Type, c.Name);
            }

            foreach ((string AccountName, string CategoryName, OperationType Type, decimal Amount, DateTime Date, string
                     ? Description) o in parsedText.Operations)
            {
                Category cat = catRepo.GetByName(o.CategoryName);
                BankAccount acc = accRepo.GetAll()
                    .First(a => a.Name.Equals(o.AccountName, StringComparison.OrdinalIgnoreCase));
                operationsFacade.Create(o.Type, acc.Id, cat.Id, o.Amount, o.Date, o.Description);
            }
        }

        protected abstract FinanceData Parse(string text);
    }
}