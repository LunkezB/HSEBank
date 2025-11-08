namespace HSEBank.Commands.UtilityCommand
{
    public sealed class RepeatLastCommand(CommandManager mgr) : ICommand
    {
        public string Name => "Повторить последнюю команду";

        public void Execute()
        {
            mgr.RepeatLast();
        }
    }
}