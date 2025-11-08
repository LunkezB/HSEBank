namespace HSEBank.Commands
{
    public sealed class CommandManager
    {
        private readonly Stack<IUndoableCommand> _redo = new();
        private readonly Stack<IUndoableCommand> _undo = new();

        private ICommand? _lastCommand;

        public void Execute(IUndoableCommand command)
        {
            command.Execute();
            _undo.Push(command);
            _redo.Clear();
            _lastCommand = command;
        }

        public void Execute(ICommand command)
        {
            command.Execute();
            _lastCommand = command;
        }

        public void Undo()
        {
            if (_undo.Count == 0)
            {
                Console.WriteLine("Нечего отменять.");
                return;
            }

            IUndoableCommand cmd = _undo.Pop();
            cmd.Undo();
            _redo.Push(cmd);
        }

        public void Redo()
        {
            if (_redo.Count == 0)
            {
                Console.WriteLine("Нечего повторять.");
                return;
            }

            IUndoableCommand cmd = _redo.Pop();
            cmd.Redo();
            _undo.Push(cmd);
            _lastCommand = cmd;
        }

        public void RepeatLast()
        {
            switch (_lastCommand)
            {
                case null:
                    Console.WriteLine("Нет последней команды для повтора.");
                    return;
                case IUndoableCommand u:
                    Execute(u);
                    break;
                default:
                    _lastCommand.Execute();
                    break;
            }
        }
    }
}