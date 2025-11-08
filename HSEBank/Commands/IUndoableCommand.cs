namespace HSEBank.Commands
{
    /// <summary>Команда с поддержкой отмены/повтора.</summary>
    public interface IUndoableCommand : ICommand
    {
        void Undo();
        void Redo();
    }
}