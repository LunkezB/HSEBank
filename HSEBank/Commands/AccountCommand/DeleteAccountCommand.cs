using HSEBank.Facades;
using HSEBank.Models;
using HSEBank.Repositories;

namespace HSEBank.Commands.AccountCommand
{
    public sealed class DeleteAccountCommand(
        IAccountsFacade accountsFacade,
        IOperationsFacade operationsFacade,
        IBankAccountRepository accRepo,
        IOperationRepository opRepo,
        Guid id)
        : IUndoableCommand
    {
        private BankAccount? _backupAccount;
        private List<Operation>? _backupOps;

        public string Name => "Удалить счёт";

        public void Execute()
        {
            _backupAccount = accRepo.Get(id);
            _backupOps = opRepo.GetByAccount(id).ToList();

            accountsFacade.Delete(id);
            Console.WriteLine("Счёт удалён.");
        }

        public void Undo()
        {
            if (_backupAccount is null)
            {
                return;
            }

            accRepo.Add(_backupAccount);

            foreach (Operation op in _backupOps ?? Enumerable.Empty<Operation>())
            {
                opRepo.Add(op);
            }

            operationsFacade.RecalculateBalance(_backupAccount.Id);

            Console.WriteLine("Удаление счёта отменено (счёт и операции восстановлены).");
        }

        public void Redo()
        {
            Execute();
        }
    }
}