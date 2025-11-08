using HSEBank.Facades;
using HSEBank.Models;

namespace HSEBank.Commands.AccountCommand
{
    public sealed class CreateAccountCommand(IAccountsFacade accounts, string name, decimal initial) : IUndoableCommand
    {
        private Guid _createdId;

        public string Name => "Создать счёт";

        public void Execute()
        {
            BankAccount acc = accounts.Create(name, initial);
            _createdId = acc.Id;
            Console.WriteLine($"Создан счёт: {acc.Id} | {name} | Баланс: {initial}");
        }

        public void Undo()
        {
            if (_createdId == Guid.Empty)
            {
                return;
            }

            accounts.Delete(_createdId);
            Console.WriteLine("Создание счёта отменено (счёт удалён).");
        }

        public void Redo()
        {
            Execute();
        }
    }
}