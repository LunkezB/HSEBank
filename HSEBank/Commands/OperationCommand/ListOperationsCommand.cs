using HSEBank.Facades;
using HSEBank.Models;
using HSEBank.Repositories;

namespace HSEBank.Commands.OperationCommand
{
    public sealed class ListOperationsCommand(
        IOperationsFacade ops,
        IBankAccountRepository accRepo,
        ICategoryRepository catRepo)
        : ICommand
    {
        public string Name => "Список операций";

        public void Execute()
        {
            List<Operation> all = ops.GetAll().ToList();
            if (all.Count == 0)
            {
                Console.WriteLine("Операций нет.");
                return;
            }

            Console.WriteLine(
                "ID                                   | Дата       | Тип     | Счёт                      | Категория                 | Сумма           | Описание");
            Console.WriteLine(
                "-------------------------------------+------------+---------+---------------------------+---------------------------+-----------------+-------------------------");

            foreach (Operation o in all)
            {
                string accName = AccName(o.BankAccountId);
                string catName = CatName(o.CategoryId);
                string typeStr = o.Type == OperationType.Income ? "income" : "expense";
                string dateStr = o.Date.ToString("dd-MM-yyyy");
                string amount = o.SignedAmount.ToString("+#,0.##;-#,0.##;0");

                Console.WriteLine(
                    $"{o.Id} | {dateStr,10} | {typeStr,-7} | {accName,-25} | {catName,-25} | {amount,15} | {o.Description}");
            }
        }

        private string AccName(Guid id)
        {
            try { return accRepo.Get(id).Name; }
            catch { return id.ToString(); }
        }

        private string CatName(Guid id)
        {
            try { return catRepo.Get(id).Name; }
            catch { return id.ToString(); }
        }
    }
}