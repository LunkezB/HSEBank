using HSEBank.Models;
using System.Globalization;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace HSEBank.Export
{
    public sealed class JsonExporter : IExportVisitor
    {
        private readonly List<BankAccount> _accs = [];
        private readonly List<Category> _cats = [];
        private readonly List<Operation> _ops = [];

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
            var obj = new
            {
                accounts = _accs.Select(a => new { a.Id, a.Name, a.Balance }),
                categories = _cats.Select(c => new { c.Id, c.Name, type = c.Type.ToString().ToLowerInvariant() }),
                operations = _ops.Select(o => new
                {
                    o.Id,
                    type = o.Type.ToString().ToLowerInvariant(),
                    bankAccountId = o.BankAccountId,
                    o.Amount,
                    date = o.Date.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    o.Description,
                    categoryId = o.CategoryId
                })
            };
            return JsonSerializer.Serialize(obj,
                new JsonSerializerOptions
                {
                    WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                });
        }
    }
}