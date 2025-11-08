namespace HSEBank.Commands.UtilityCommand
{
    public sealed class RedoCommand(CommandManager mgr) : ICommand
    {
        public string Name => "Redo";

        public void Execute()
        {
            mgr.Redo();
        }
    }
}