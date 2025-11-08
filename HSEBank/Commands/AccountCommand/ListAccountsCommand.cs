using HSEBank.Facades;
using HSEBank.Models;

namespace HSEBank.Commands.AccountCommand
{
    public sealed class ListAccountsCommand(IAccountsFacade accounts) : ICommand
    {
        public string Name => "Список счетов";

        public void Execute()
        {
            List<BankAccount> all = accounts.GetAll().ToList();
            if (all.Count == 0)
            {
                Console.WriteLine("Счетов нет.");
                return;
            }

            Console.WriteLine("ID                                   | Имя                       | Баланс");
            Console.WriteLine("-------------------------------------+---------------------------+-----------------");
            foreach (BankAccount a in all)
            {
                Console.WriteLine($"{a.Id} | {a.Name,-25} | {a.Balance,15}");
            }
        }
    }
}