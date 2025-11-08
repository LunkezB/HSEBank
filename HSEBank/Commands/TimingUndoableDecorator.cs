using System.Diagnostics;

namespace HSEBank.Commands
{
    /// <summary>Декоратор, считающий время Execute/Undo/Redo.</summary>
    public sealed class TimingUndoableDecorator(IUndoableCommand inner) : TimingDecorator(inner), IUndoableCommand
    {
        public void Undo()
        {
            Stopwatch sw = Stopwatch.StartNew();
            inner.Undo();
            sw.Stop();
            Console.WriteLine($"[Статистика] «{Name}::Undo» = {sw.ElapsedMilliseconds} мс");
        }

        public void Redo()
        {
            Stopwatch sw = Stopwatch.StartNew();
            inner.Redo();
            sw.Stop();
            Console.WriteLine($"[Статистика] «{Name}::Redo» = {sw.ElapsedMilliseconds} мс");
        }
    }
}