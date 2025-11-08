namespace HSEBank.Commands.UtilityCommand
{
    public sealed class UndoCommand(CommandManager mgr) : ICommand
    {
        public string Name => "Undo";

        public void Execute()
        {
            mgr.Undo();
        }
    }
}