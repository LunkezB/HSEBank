using System.Diagnostics;

namespace HSEBank.Commands
{
    public class TimingDecorator(ICommand inner) : ICommand
    {
        public string Name => inner.Name;

        public void Execute()
        {
            Stopwatch sw = Stopwatch.StartNew();
            inner.Execute();
            sw.Stop();
            Console.WriteLine($"[Статистика] «{Name}» выполнена за {sw.ElapsedMilliseconds} мс");
        }
    }
}