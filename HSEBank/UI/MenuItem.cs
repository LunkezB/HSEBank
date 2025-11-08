using HSEBank.Commands;

namespace HSEBank.UI
{
    public sealed record MenuItem(string Key, string Title, Func<ICommand> BuildCommand);
}