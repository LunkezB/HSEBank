using System.Globalization;

namespace HSEBank.UI
{
    public sealed class ConsoleInput : IInput
    {
        public string Ask(string prompt)
        {
            Console.Write(prompt);
            string? s = Console.ReadLine();
            return string.IsNullOrWhiteSpace(s)
                ? throw new FormatException("Значение не должно быть пустым.")
                : s.Trim();
        }

        public string? AskOptional(string prompt)
        {
            Console.Write(prompt);
            string? s = Console.ReadLine();
            return string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        }

        public Guid AskGuid(string prompt)
        {
            Console.Write(prompt);
            string? s = Console.ReadLine();
            return !Guid.TryParse(s, out Guid id) ? throw new FormatException("Некорректный GUID.") : id;
        }

        public int AskInt(string prompt)
        {
            Console.Write(prompt);
            return !int.TryParse(Console.ReadLine(), out int v)
                ? throw new FormatException("Ожидалось целое число.")
                : v;
        }

        public decimal AskDecimal(string prompt, IFormatProvider? fp = null)
        {
            Console.Write(prompt);
            fp ??= CultureInfo.InvariantCulture;
            return !decimal.TryParse(Console.ReadLine(), NumberStyles.Number, fp, out decimal v)
                ? throw new FormatException("Ожидалось число (используйте точку как разделитель для дробной части).")
                : v;
        }

        public DateTime AskDate(string prompt, string fmt, IFormatProvider? fp = null)
        {
            Console.Write(prompt);
            fp ??= CultureInfo.InvariantCulture;
            return !DateTime.TryParseExact(Console.ReadLine(), fmt, fp, DateTimeStyles.None, out DateTime d)
                ? throw new FormatException($"Ожидалась дата в формате {fmt}.")
                : d;
        }
    }
}