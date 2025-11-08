using HSEBank.Facades;

namespace HSEBank.Commands.AccountCommand
{
    public sealed class RenameAccountCommand(IAccountsFacade accounts, Guid id, string newName) : IUndoableCommand
    {
        private string? _oldName;

        public string Name => "Переименовать счёт";

        public void Execute()
        {
            _oldName = accounts.GetAll().First(a => a.Id == id).Name;
            accounts.Rename(id, newName);
            Console.WriteLine("Счёт переименован.");
        }

        public void Undo()
        {
            if (_oldName is null)
            {
                return;
            }

            accounts.Rename(id, _oldName);
            Console.WriteLine("Переименование отменено.");
        }

        public void Redo()
        {
            accounts.Rename(id, newName);
            Console.WriteLine("Переименование повторено.");
        }
    }
}