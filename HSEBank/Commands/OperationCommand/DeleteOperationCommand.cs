using HSEBank.Facades;
using HSEBank.Models;

namespace HSEBank.Commands.OperationCommand
{
    public sealed class DeleteOperationCommand(IOperationsFacade ops, Guid opId) : IUndoableCommand
    {
        private Operation? _backup;

        public string Name => "Удалить операцию";

        public void Execute()
        {
            _backup = ops.Get(opId);
            ops.Delete(opId);
            Console.WriteLine("Операция удалена.");
        }

        public void Undo()
        {
            if (_backup is null)
            {
                return;
            }

            Operation restored = ops.Create(_backup.Type, _backup.BankAccountId, _backup.CategoryId,
                _backup.Amount, _backup.Date, _backup.Description);
            Console.WriteLine($"Удаление операции отменено (восстановлено как {restored.Id}).");
        }

        public void Redo()
        {
            Execute();
        }
    }
}