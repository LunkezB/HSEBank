using HSEBank.Facades;
using HSEBank.Models;

namespace HSEBank.Commands.OperationCommand
{
    public sealed class CreateOperationCommand(
        IOperationsFacade ops,
        OperationType type,
        Guid accountId,
        Guid categoryId,
        decimal amount,
        DateTime date,
        string? desc)
        : IUndoableCommand
    {
        private Guid _opId;

        public string Name => "Создать операцию";

        public void Execute()
        {
            Operation op = ops.Create(type, accountId, categoryId, amount, date, desc);
            _opId = op.Id;
            Console.WriteLine($"Создана операция: {type} {amount} на {date:dd-MM-yyyy}.");
        }

        public void Undo()
        {
            if (_opId == Guid.Empty)
            {
                return;
            }

            ops.Delete(_opId);
            Console.WriteLine("Создание операции отменено (операция удалена).");
        }

        public void Redo()
        {
            Execute();
        }
    }
}