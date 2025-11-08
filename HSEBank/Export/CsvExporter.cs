using HSEBank.Models;
using System.Globalization;
using System.Text;

namespace HSEBank.Export
{
    public sealed class CsvExporter : IExportVisitor
    {
        private readonly List<BankAccount> _accs = [];
        private readonly List<Category> _cats = [];
        private readonly List<Operation> _ops = [];
        private readonly StringBuilder _sb = new();

        public void Visit(BankAccount acc)
        {
            _accs.Add(acc);
        }

        public void Visit(Category cat)
        {
            _cats.Add(cat);
        }

        public void Visit(Operation op)
        {
            _ops.Add(op);
        }

        public string GetResult()
        {
            _sb.AppendLine("[accounts]");
            _sb.AppendLine("Id,Name,Balance");
            foreach (BankAccount a in _accs)
            {
                _sb.AppendLine($"{a.Id},{a.Name},{a.Balance}");
            }

            _sb.AppendLine("[categories]");
            _sb.AppendLine("Id,Name,Type");
            foreach (Category c in _cats)
            {
                _sb.AppendLine($"{c.Id},{c.Name},{c.Type.ToString().ToLowerInvariant()}");
            }

            _sb.AppendLine("[operations]");
            _sb.AppendLine("Id,Type,BankAccountId,Amount,Date,Description,CategoryId");
            foreach (Operation o in _ops)
            {
                _sb.AppendLine(
                    $"{o.Id},{o.Type.ToString().ToLowerInvariant()},{o.BankAccountId},{o.Amount},{o.Date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)},{o.Description},{o.CategoryId}");
            }

            return _sb.ToString();
        }
    }
}